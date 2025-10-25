variable "project_id" {
  description = "GCP project ID"
  type        = string
}

variable "region" {
  description = "GCP region"
  type        = string
  default     = "us-central1"
}

variable "artifact_repo" {
  description = "Artifact Registry repository name"
  type        = string
  default     = "paparchar-backend"
}

variable "service_name" {
  description = "Cloud Run service name"
  type        = string
  default     = "paparchar-api"
}

variable "image" {
  description = "Container image to deploy (Artifact Registry URI). Leave empty during provision."
  type        = string
  default     = ""
}

variable "allow_unauthenticated" {
  description = "Allow public (unauthenticated) access to Cloud Run"
  type        = bool
  default     = true
}

variable "create_cloudsql" {
  description = "Whether to create Cloud SQL instance, database and user"
  type        = bool
  default     = true
}

variable "db_instance_name" {
  description = "Cloud SQL instance name"
  type        = string
  default     = "paparchar-sql"
}

variable "db_tier" {
  description = "Cloud SQL machine tier"
  type        = string
  default     = "db-f1-micro"
}

variable "db_name" {
  description = "Database name"
  type        = string
  default     = "paparchar"
}

variable "db_user" {
  description = "Database user"
  type        = string
  default     = "app_user"
}

variable "bucket_name" {
  description = "GCS bucket name for images"
  type        = string
}

variable "migrate_image" {
  description = "Container image for EF Core migrations job"
  type        = string
  default     = ""
}
