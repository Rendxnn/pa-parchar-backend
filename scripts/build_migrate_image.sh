#!/usr/bin/env bash
set -euo pipefail

PROJECT_ID=${PROJECT_ID:?set PROJECT_ID}
REGION=${REGION:-us-central1}
REPO=${REPO:-paparchar-backend}
IMAGE_NAME=${IMAGE_NAME:-paparchar-migrate}
TAG=${TAG:-$(date +%Y%m%d%H%M%S)}
IMAGE_URI="$REGION-docker.pkg.dev/$PROJECT_ID/$REPO/$IMAGE_NAME:$TAG"

echo "Building migrations image $IMAGE_URI ..."
docker build -f Dockerfile.migrate -t "$IMAGE_URI" .

echo "Pushing image..."
gcloud auth configure-docker "$REGION-docker.pkg.dev" -q
docker push "$IMAGE_URI"

echo "Export MIGRATE_IMAGE=$IMAGE_URI"
echo "MIGRATE_IMAGE=$IMAGE_URI"

