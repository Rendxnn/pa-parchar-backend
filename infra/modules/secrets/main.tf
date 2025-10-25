resource "google_secret_manager_secret" "db_conn" {
  secret_id = "db-connection-string"
  replication {
    auto {}
  }
}

resource "google_secret_manager_secret_version" "db_conn_v" {
  count       = var.db_connection_string_value != null ? 1 : 0
  secret      = google_secret_manager_secret.db_conn.id
  secret_data = var.db_connection_string_value
}
