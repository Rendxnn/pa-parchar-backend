resource "google_sql_database_instance" "pg" {
  name             = var.instance
  database_version = "POSTGRES_15"
  region           = var.region

  settings {
    tier = var.tier
    ip_configuration {
      ipv4_enabled    = true
    }
  }
}

resource "google_sql_database" "db" {
  name     = var.db_name
  instance = google_sql_database_instance.pg.name
}

resource "random_password" "db_password" {
  length  = 24
  special = true
}

resource "google_sql_user" "app" {
  instance = google_sql_database_instance.pg.name
  name     = var.db_user
  password = random_password.db_password.result
}

locals {
  connection_name = google_sql_database_instance.pg.connection_name
  # Unix socket connection string for Npgsql
  connection_string = "Host=/cloudsql/${local.connection_name};Database=${var.db_name};Username=${var.db_user};Password=${random_password.db_password.result}"
}
