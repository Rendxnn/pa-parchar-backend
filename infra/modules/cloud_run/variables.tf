variable "project_id" { type = string }
variable "location"   { type = string }
variable "service_name" { type = string }
variable "image" { type = string }
variable "allow_unauthenticated" { type = bool }
variable "service_account_email" { type = string }
variable "bucket_name" { type = string }
variable "db_connection_secret" { type = string }
variable "cloud_sql_connection" { type = string }

