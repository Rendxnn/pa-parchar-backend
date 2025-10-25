output "artifact_registry_repo_url" {
  value       = module.artifact_registry.repository_url
  description = "Artifact Registry repository URL"
}

output "cloud_run_url" {
  value       = var.image != "" && length(module.cloud_run) > 0 ? module.cloud_run[0].url : null
  description = "Cloud Run service URL"
}

output "cloud_sql_connection_name" {
  value       = var.create_cloudsql ? module.cloud_sql[0].connection_name : null
  description = "Cloud SQL instance connection name"
}

output "db_connection_string_secret" {
  value       = module.secrets.db_connection_string_secret_id
  description = "Secret Manager secret ID for DB connection string"
}

output "bucket_name" {
  value       = module.storage.bucket_name
  description = "GCS bucket for images"
}
