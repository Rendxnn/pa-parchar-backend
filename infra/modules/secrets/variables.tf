variable "project_id" { type = string }
variable "db_connection_string_value" {
  type        = string
  description = "Full DB connection string to store in Secret Manager"
  default     = null
}

