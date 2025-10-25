#!/usr/bin/env bash
set -euo pipefail

PROJECT_ID=${PROJECT_ID:?set PROJECT_ID}
REGION=${REGION:-us-central1}
REPO=${REPO:-paparchar-backend}
IMAGE_NAME=${IMAGE_NAME:-paparchar-api}
TAG=${TAG:-$(date +%Y%m%d%H%M%S)}
IMAGE_URI="$REGION-docker.pkg.dev/$PROJECT_ID/$REPO/$IMAGE_NAME:$TAG"

echo "Building image $IMAGE_URI ..."
docker build -f PaParchar.Api/Dockerfile -t "$IMAGE_URI" .

echo "Pushing image..."
gcloud auth configure-docker "$REGION-docker.pkg.dev" -q
docker push "$IMAGE_URI"

echo "Exporting IMAGE=$IMAGE_URI"
echo "IMAGE=$IMAGE_URI"

