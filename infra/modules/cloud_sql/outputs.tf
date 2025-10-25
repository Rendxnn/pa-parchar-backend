output "connection_name" {
  value = google_sql_database_instance.pg.connection_name
}

output "db_connection_string" {
  value     = local.connection_string
  sensitive = true
}

