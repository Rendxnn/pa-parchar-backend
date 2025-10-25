locals {
  location = var.region
}

resource "google_project_service" "services" {
  for_each = toset([
    "run.googleapis.com",
    "artifactregistry.googleapis.com",
    "sqladmin.googleapis.com",
    "secretmanager.googleapis.com",
    "logging.googleapis.com",
    "cloudbuild.googleapis.com",
    "iam.googleapis.com"
  ])
  service = each.key
}

module "artifact_registry" {
  source      = "./modules/artifact_registry"
  project_id  = var.project_id
  location    = local.location
  repo_name   = var.artifact_repo
  depends_on  = [google_project_service.services]
}

module "service_accounts" {
  source     = "./modules/service_accounts"
  project_id = var.project_id
  sa_name    = "run-sa"
}

module "storage" {
  source      = "./modules/storage"
  project_id  = var.project_id
  location    = local.location
  bucket_name = var.bucket_name
}

module "cloud_sql" {
  count       = var.create_cloudsql ? 1 : 0
  source      = "./modules/cloud_sql"
  project_id  = var.project_id
  region      = local.location
  instance    = var.db_instance_name
  tier        = var.db_tier
  db_name     = var.db_name
  db_user     = var.db_user
}

module "secrets" {
  source                     = "./modules/secrets"
  project_id                 = var.project_id
  db_connection_string_value = var.create_cloudsql ? module.cloud_sql[0].db_connection_string : null
  # When create_cloudsql=false, you must update the secret value manually post-apply.
}

module "iam" {
  source                = "./modules/iam"
  project_id            = var.project_id
  service_account_email = module.service_accounts.email
  bucket_name           = module.storage.bucket_name
}

module "cloud_run" {
  count                  = var.image != "" ? 1 : 0
  source                 = "./modules/cloud_run"
  project_id             = var.project_id
  location               = local.location
  service_name           = var.service_name
  image                  = var.image
  allow_unauthenticated  = var.allow_unauthenticated
  service_account_email  = module.service_accounts.email
  bucket_name            = module.storage.bucket_name
  db_connection_secret   = module.secrets.db_connection_string_secret_id
  cloud_sql_connection   = var.create_cloudsql ? module.cloud_sql[0].connection_name : null
  depends_on             = [google_project_service.services]
}

# Optional: Cloud Run Job to initialize DB (PostGIS extension)
module "db_init_job" {
  count                = var.create_cloudsql ? 1 : 0
  source               = "./modules/cloud_run_job"
  project_id           = var.project_id
  location             = local.location
  job_name             = "db-init-job"
  image                = "postgres:16" # uses psql client
  service_account_email = module.service_accounts.email
  cloud_sql_connection = module.cloud_sql[0].connection_name
  db_connection_secret = module.secrets.db_connection_string_secret_id
  command              = ["/bin/sh", "-c"]
  args                 = ["psql \"$CONN\" -c 'CREATE EXTENSION IF NOT EXISTS postgis;' || true"]
}

# Cloud Run Job to run EF migrations
module "db_migrate_job" {
  count                 = var.migrate_image != "" && var.create_cloudsql ? 1 : 0
  source                = "./modules/cloud_run_job"
  project_id            = var.project_id
  location              = local.location
  job_name              = "db-migrate-job"
  image                 = var.migrate_image
  service_account_email = module.service_accounts.email
  cloud_sql_connection  = module.cloud_sql[0].connection_name
  db_connection_secret  = module.secrets.db_connection_string_secret_id
  command               = ["/bin/sh", "-c"]
  args                  = ["cd /work && dotnet ef database update --project PaParchar.Infrastructure --startup-project PaParchar.Api --connection \"$CONN\" "]
}
