resource "google_cloud_run_v2_job" "job" {
  name     = var.job_name
  location = var.location

  template {
    template {
      service_account = var.service_account_email
      containers {
        image   = var.image
        command = var.command
        args    = var.args
        env {
          name = "CONN"
          value_source {
            secret_key_ref {
              secret  = var.db_connection_secret
              version = "latest"
            }
          }
        }

        volume_mounts {
          name       = "cloudsql"
          mount_path = "/cloudsql"
        }
      }
      volumes {
        name = "cloudsql"
        cloud_sql_instance {
          instances = [var.cloud_sql_connection]
        }
      }
    }
  }
}

