#!/usr/bin/env bash
set -euo pipefail

BASE_URL=${BASE_URL:?set BASE_URL to Cloud Run URL}

docker run --rm -i -e BASE_URL="$BASE_URL" grafana/k6 run - < k6/simple-get.js

