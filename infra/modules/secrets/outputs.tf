output "db_connection_string_secret_id" {
  value = google_secret_manager_secret.db_conn.id
}

