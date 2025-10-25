#!/usr/bin/env bash
set -euo pipefail

PROJECT_ID=${PROJECT_ID:?set PROJECT_ID}
REGION=${REGION:-us-central1}
IMAGE=${IMAGE:?set IMAGE from build_push.sh output}
BUCKET_NAME=${BUCKET_NAME:?set BUCKET_NAME}
MIGRATE_IMAGE=${MIGRATE_IMAGE:-}

echo "Applying Terraform with image=$IMAGE and bucket=$BUCKET_NAME ..."
terraform -chdir=infra apply -auto-approve \
  -var="project_id=$PROJECT_ID" \
  -var="region=$REGION" \
  -var="image=$IMAGE" \
  -var="bucket_name=$BUCKET_NAME" \
  -var="migrate_image=$MIGRATE_IMAGE"

echo "Deployment done. Cloud Run URL:"
terraform -chdir=infra output cloud_run_url
