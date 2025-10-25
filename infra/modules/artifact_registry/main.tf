resource "google_artifact_registry_repository" "repo" {
  location      = var.location
  repository_id = var.repo_name
  description   = "Artifact Registry for PaParchar backend"
  format        = "DOCKER"
}

