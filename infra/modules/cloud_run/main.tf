resource "google_cloud_run_v2_service" "service" {
  name     = var.service_name
  location = var.location

  template {
    service_account = var.service_account_email
    containers {
      image = var.image
      env {
        name  = "ASPNETCORE_URLS"
        value = "http://0.0.0.0:8080"
      }
      env {
        name  = "GoogleCloudStorage__BucketName"
        value = var.bucket_name
      }
      env {
        name  = "GoogleCloudStorage__CredentialsFilePath"
        value = ""
      }
      env {
        name = "ConnectionStrings__PostgreSQLConnection"
        value_source {
          secret_key_ref {
            secret  = var.db_connection_secret
            version = "latest"
          }
        }
      }

      ports { container_port = 8080 }

      dynamic "volume_mounts" {
        for_each = var.cloud_sql_connection != null ? [1] : []
        content {
          name       = "cloudsql"
          mount_path = "/cloudsql"
        }
      }
    }

    dynamic "volumes" {
      for_each = var.cloud_sql_connection != null ? [1] : []
      content {
        name = "cloudsql"
        cloud_sql_instance {
          instances = [var.cloud_sql_connection]
        }
      }
    }
  }

  ingress = "INGRESS_TRAFFIC_ALL"
}

resource "google_cloud_run_v2_service_iam_member" "invoker" {
  count    = var.allow_unauthenticated ? 1 : 0
  location = google_cloud_run_v2_service.service.location
  name     = google_cloud_run_v2_service.service.name
  role     = "roles/run.invoker"
  member   = "allUsers"
}

