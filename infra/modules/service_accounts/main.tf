resource "google_service_account" "run_sa" {
  account_id   = var.sa_name
  display_name = "Cloud Run Service Account"
}

