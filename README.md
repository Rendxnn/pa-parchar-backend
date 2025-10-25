# PaParchar Backend – Despliegue en GCP con Terraform y Cloud Run

Este repositorio contiene el backend (.NET 8 Web API) y todo lo necesario para provisionar y desplegar en Google Cloud Platform usando Terraform, Artifact Registry, Cloud Run, Cloud SQL (Postgres + PostGIS), Secret Manager y Cloud Logging. También incluye un esqueleto de pruebas de carga con k6.

## Arquitectura (resumen)
- Imagen Docker de la API publicada en Artifact Registry.
- Servicio en Cloud Run (público sin autenticación) en `us-central1`.
- Base de datos en Cloud SQL (Postgres 15, conexión mediante conector de Cloud SQL por socket Unix).
- Secret Manager para la cadena de conexión (`db-connection-string`).
- GCS (bucket) para almacenamiento de imágenes (la API usa `GCPBucketService`).
- Jobs de Cloud Run:
  - `db-init-job`: habilita PostGIS.
  - `db-migrate-job`: corre migraciones EF Core.
- Observabilidad: Cloud Logging (nativo de Cloud Run).

## Requisitos previos
- Ubuntu o Linux con:
  - Docker y acceso al daemon.
  - Terraform >= 1.5.
  - gcloud CLI autenticado (billing activo en el proyecto). 
- Acceso de tu usuario a: habilitar APIs, crear IAM, Artifact Registry, Cloud SQL, Secret Manager, Cloud Run.

Consulta guía paso a paso para Ubuntu en `docs/RUNBOOK_UBUNTU.md`.

## Estructura clave
- `PaParchar.Api/Program.cs`: registra `IFileStorageService` como `GCPBucketService` (usa credenciales por defecto de la cuenta de servicio en Cloud Run).
- `infra/` (Terraform):
  - `providers.tf`, `variables.tf`, `main.tf`, `outputs.tf`.
  - `modules/`:
    - `artifact_registry/`: repositorio Docker.
    - `service_accounts/`: SA de Cloud Run.
    - `iam/`: roles mínimos (artifactregistry.reader, cloudsql.client, secretAccessor, storage.objectAdmin).
    - `storage/`: bucket GCS para imágenes.
    - `cloud_sql/`: instancia Postgres, DB y usuario (opcional con `create_cloudsql`).
    - `secrets/`: secreto `db-connection-string` (y versión si Terraform crea Cloud SQL).
    - `cloud_run/`: servicio Cloud Run v2, con conector de Cloud SQL y secretos como env.
    - `cloud_run_job/`: jobs para init PostGIS y migraciones EF.
- `scripts/`:
  - `provision.sh`: habilita APIs y `terraform init/plan/apply` para base.
  - `build_push.sh`: build/push de imagen de API.
  - `build_migrate_image.sh`: build/push de imagen para migraciones (`dotnet-ef`).
  - `deploy.sh`: `terraform apply` de Cloud Run + Jobs (requiere imágenes).
  - `run_jobs.sh`: ejecuta `db-init-job` y `db-migrate-job`.
  - `test_k6.sh`: corre k6 contra `/api/parche`.
  - `destroy.sh`: `terraform destroy` de todo.
- `Makefile`: atajos para todo el flujo (`provision`, `build`, `build-migrate`, `deploy`, `run-jobs`, `test`, `outputs`, `destroy`).
- `Dockerfile.migrate`: imagen SDK + dotnet-ef para correr migraciones en un Job.
- `k6/simple-get.js`: test simple contra `GET /api/parche`.

## Variables importantes (Terraform)
- `project_id` (string): ID de proyecto GCP.
- `region` (string): región (default: `us-central1`).
- `artifact_repo` (string): nombre del repo de Artifact Registry (default: `paparchar-backend`).
- `service_name` (string): nombre del servicio en Cloud Run (default: `paparchar-api`).
- `bucket_name` (string): nombre global único para el bucket GCS (requerido en provision/deploy).
- `create_cloudsql` (bool): crea o no Cloud SQL (default: true).
- `db_instance_name` (string), `db_tier` (string), `db_name` (string), `db_user` (string): parámetros de Cloud SQL.
- `image` (string): URI de imagen para Cloud Run (vacío en provision, requerido en deploy).
- `migrate_image` (string): URI de imagen para Job de migraciones (requerido en deploy para crear el job).

Notas:
- En `infra/main.tf`, el módulo de Cloud Run tiene `count = var.image != "" ? 1 : 0`; por eso en provision no se crea Cloud Run ni se pide `image`.

## Flujo de despliegue (paso a paso)
No uses `sudo` con `make/terraform/gcloud`.

1) Preparación
```
chmod +x scripts/*.sh
gcloud auth login
export PROJECT_ID="<tu-proyecto>"
gcloud config set project "$PROJECT_ID"
export REGION="us-central1"
export BUCKET_NAME="paparchar-images-$PROJECT_ID-$(date +%s)"  # único
```

2) Provisionar base (repos, SA/IAM, bucket, Cloud SQL, secreto)
```
make provision PROJECT_ID=$PROJECT_ID REGION=$REGION BUCKET_NAME=$BUCKET_NAME
```
- Cloud SQL puede tardar 5–15 min. No interrumpas.

3) Construir y subir imagen de la API
```
make build PROJECT_ID=$PROJECT_ID REGION=$REGION
# Copia el valor IMAGE=... que imprime al final
```

4) Construir y subir imagen de migraciones
```
make build-migrate PROJECT_ID=$PROJECT_ID REGION=$REGION
# Copia MIGRATE_IMAGE=...
```

5) Desplegar Cloud Run + Jobs (usa las URIs anteriores)
```
make deploy PROJECT_ID=$PROJECT_ID REGION=$REGION BUCKET_NAME=$BUCKET_NAME IMAGE="$IMAGE" MIGRATE_IMAGE="$MIGRATE_IMAGE"
```

6) Inicializar DB y migraciones
```
make run-jobs PROJECT_ID=$PROJECT_ID REGION=$REGION
```
- Ejecuta `db-init-job` (PostGIS) y `db-migrate-job` (EF).

7) Obtener URL y probar
```
make outputs
CLOUD_RUN_URL=$(terraform -chdir=infra output -raw cloud_run_url)
curl "$CLOUD_RUN_URL/api/parche"
# o abrir "$CLOUD_RUN_URL/swagger"
```

8) Pruebas de carga k6 (opcional)
```
make test PROJECT_ID=$PROJECT_ID
```

9) Destruir el entorno
```
make destroy PROJECT_ID=$PROJECT_ID REGION=$REGION
```

## Scripts (resumen)
- `scripts/provision.sh`: habilita APIs y aplica Terraform para base (`project_id`, `region`, `bucket_name`, `create_cloudsql`).
- `scripts/build_push.sh`: build/push de `PaParchar.Api/Dockerfile` a Artifact Registry. Imprime `IMAGE=<uri>`.
- `scripts/build_migrate_image.sh`: build/push de `Dockerfile.migrate`. Imprime `MIGRATE_IMAGE=<uri>`.
- `scripts/deploy.sh`: `terraform apply` con `image`, `migrate_image` y `bucket_name` para crear Cloud Run y Jobs.
- `scripts/run_jobs.sh`: ejecuta `db-init-job` y `db-migrate-job` en Cloud Run.
- `scripts/test_k6.sh`: ejecuta `k6/simple-get.js` con Docker contra `BASE_URL`.
- `scripts/destroy.sh`: `terraform destroy` del entorno.

Makefile mapea estos scripts a targets con parámetros (`PROJECT_ID`, `REGION`, `BUCKET_NAME`, `IMAGE`, `MIGRATE_IMAGE`).

## Cómo funciona la conexión a DB
- Terraform crea (opcionalmente) Cloud SQL y un usuario/DB, y genera un secreto `db-connection-string` con formato:
  `Host=/cloudsql/PROJECT:REGION:INSTANCE;Database=paparchar;Username=app_user;Password=<pwd>`
- Cloud Run monta el conector de Cloud SQL como volumen `/cloudsql` y recibe la cadena de conexión desde el secreto como env `ConnectionStrings__PostgreSQLConnection`.
- La app .NET toma esa variable como `builder.Configuration.GetConnectionString("PostgreSQLConnection")` (ya configurado en Program.cs).

## Subida de archivos
- En Cloud Run, se usa GCS. `GCPBucketService` toma el nombre del bucket desde env y usa Application Default Credentials (la SA de Cloud Run tiene permisos de `storage.objectAdmin` sobre el bucket).

## Solución de problemas
- Artifact Registry repo not found al hacer push:
  - Asegúrate de haber corrido `make provision` antes de `make build`.
- Cloud SQL tarda mucho (7–15 min):
  - Es normal. No interrumpas `terraform apply`.
  - Si cortaste y la instancia siguió en GCP, puedes importarla al estado o borrarla y reintentar.
- Error 409 Secret/Bucket ya existen:
  - O importas los recursos al estado, o los borras y reintentas provision.
- Prompts de Terraform pidiendo `image` en provision:
  - Ya está resuelto: `image` tiene default `""` y Cloud Run se crea en deploy.
- Problemas por usar sudo:
  - Evita `sudo` con make/terraform/gcloud. Si lo usaste, corrige ownership: `sudo chown -R "$USER":"$USER" infra scripts k6 docs Makefile`.

## Notas finales
- El servicio es público (allow_unauthenticated=true) y accesible por URL de Cloud Run.
- Puedes adaptar región/tiers si cambias los valores en `infra/variables.tf` o pasas `-var`.
- Si prefieres no exponer públicamente, cambia `allow_unauthenticated=false` y maneja IAM/IAP.

