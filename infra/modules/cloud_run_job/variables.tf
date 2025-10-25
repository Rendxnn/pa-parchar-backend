variable "project_id" { type = string }
variable "location"   { type = string }
variable "job_name"   { type = string }
variable "image"      { type = string }
variable "service_account_email" { type = string }
variable "cloud_sql_connection" { type = string }
variable "db_connection_secret" { type = string }
variable "command" { type = list(string) }
variable "args" { type = list(string) }

