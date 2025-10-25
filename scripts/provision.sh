#!/usr/bin/env bash
set -euo pipefail

PROJECT_ID=${PROJECT_ID:?set PROJECT_ID}
REGION=${REGION:-us-central1}

echo "Enabling required services..."
gcloud services enable \
  run.googleapis.com \
  artifactregistry.googleapis.com \
  sqladmin.googleapis.com \
  secretmanager.googleapis.com \
  logging.googleapis.com \
  cloudbuild.googleapis.com \
  iam.googleapis.com --project "$PROJECT_ID"

echo "Initializing Terraform..."
terraform -chdir=infra init

echo "Planning Terraform..."
terraform -chdir=infra plan -var="project_id=$PROJECT_ID" -var="region=$REGION"

echo "Applying Terraform..."
terraform -chdir=infra apply -auto-approve -var="project_id=$PROJECT_ID" -var="region=$REGION" "$@"

