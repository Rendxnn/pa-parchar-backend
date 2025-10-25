resource "google_storage_bucket" "images" {
  name          = var.bucket_name
  location      = var.location
  force_destroy = false
  uniform_bucket_level_access = true
  lifecycle_rule {
    action { type = "Delete" }
    condition { age = 365 }
  }
}

