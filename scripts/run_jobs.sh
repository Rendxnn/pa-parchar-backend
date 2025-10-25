#!/usr/bin/env bash
set -euo pipefail

PROJECT_ID=${PROJECT_ID:?set PROJECT_ID}
REGION=${REGION:-us-central1}

echo "Executing db-init-job (PostGIS extension)..."
gcloud run jobs execute db-init-job --region "$REGION" --project "$PROJECT_ID" || true

echo "Executing db-migrate-job (EF migrations)..."
gcloud run jobs execute db-migrate-job --region "$REGION" --project "$PROJECT_ID"

