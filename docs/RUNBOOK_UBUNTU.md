# Runbook Ubuntu - Provisionar y Desplegar PaParchar en GCP

1) Instalar dependencias (si no las tienes)
- Docker
  - sudo apt-get update && sudo apt-get install -y ca-certificates curl gnupg
  - sudo install -m 0755 -d /etc/apt/keyrings
  - curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
  - echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
  - sudo apt-get update && sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
  - sudo usermod -aG docker $USER && newgrp docker
- Terraform
  - sudo apt-get update && sudo apt-get install -y gnupg software-properties-common
  - wget -O- https://apt.releases.hashicorp.com/gpg | gpg --dearmor | sudo tee /usr/share/keyrings/hashicorp-archive-keyring.gpg > /dev/null
  - echo "deb [signed-by=/usr/share/keyrings/hashicorp-archive-keyring.gpg] https://apt.releases.hashicorp.com $(lsb_release -cs) main" | sudo tee /etc/apt/sources.list.d/hashicorp.list
  - sudo apt-get update && sudo apt-get install -y terraform
- gcloud CLI
  - type -p curl >/dev/null || sudo apt-get install curl -y
  - curl -O https://dl.google.com/dl/cloudsdk/channels/rapid/downloads/google-cloud-cli-474.0.0-linux-x86_64.tar.gz
  - tar -xf google-cloud-cli-474.0.0-linux-x86_64.tar.gz
  - ./google-cloud-sdk/install.sh -q
  - exec -l $SHELL
  - gcloud init

2) Autenticación y proyecto
- gcloud auth login
- gcloud config set project <PROJECT_ID>

3) Preparar el repo y permisos de scripts
- cd al directorio del proyecto
- chmod +x scripts/*.sh

4) Variables de entorno
- export PROJECT_ID=<tu-proyecto>
- export REGION=us-central1
- export BUCKET_NAME=<nombre-unico-global-para-gcs>

5) Provisionar infraestructura base
- make provision PROJECT_ID=$PROJECT_ID REGION=$REGION BUCKET_NAME=$BUCKET_NAME

6) Construir y subir imagen de la API
- make build PROJECT_ID=$PROJECT_ID REGION=$REGION
- Copia el valor IMAGE=... que imprime al final

7) Construir y subir imagen de migraciones
- make build-migrate PROJECT_ID=$PROJECT_ID REGION=$REGION
- Copia el valor MIGRATE_IMAGE=... que imprime al final

8) Desplegar Cloud Run (API + Jobs)
- make deploy PROJECT_ID=$PROJECT_ID REGION=$REGION BUCKET_NAME=$BUCKET_NAME IMAGE=<IMAGE> MIGRATE_IMAGE=<MIGRATE_IMAGE>
- Para ver la URL: make outputs

9) Inicializar PostGIS y correr migraciones
- make run-jobs PROJECT_ID=$PROJECT_ID REGION=$REGION

10) Probar la API
- Abre en navegador la URL de Cloud Run + /swagger
- curl "$(terraform -chdir=infra output -raw cloud_run_url)/api/parche"

11) Pruebas de carga (k6)
- make test PROJECT_ID=$PROJECT_ID

12) Destruir el entorno
- make destroy PROJECT_ID=$PROJECT_ID REGION=$REGION

Notas:
- El bucket GCS lo crea Terraform; debes proporcionar un nombre único (BUCKET_NAME) para evitar colisiones.
- Si no quieres que Terraform cree Cloud SQL, usa -var="create_cloudsql=false" en provision/deploy y carga el secreto db-connection-string manualmente.
- Si cambias la región o el repo, asegúrate de reconstruir y reconfigurar las imágenes.

