#!/usr/bin/env bash
set -euo pipefail

PROJECT_ID=${PROJECT_ID:?set PROJECT_ID}
REGION=${REGION:-us-central1}

terraform -chdir=infra destroy -auto-approve -var="project_id=$PROJECT_ID" -var="region=$REGION"

