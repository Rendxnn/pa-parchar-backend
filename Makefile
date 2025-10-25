.PHONY: provision build build-migrate deploy run-jobs outputs test destroy

PROJECT_ID ?= $(shell gcloud config get-value project 2>/dev/null)
REGION ?= us-central1
BUCKET_NAME ?=
IMAGE ?=
MIGRATE_IMAGE ?=

provision:
	@[ -n "$(PROJECT_ID)" ] || (echo "Set PROJECT_ID" && exit 1)
	@[ -n "$(BUCKET_NAME)" ] || (echo "Set BUCKET_NAME" && exit 1)
	PROJECT_ID=$(PROJECT_ID) REGION=$(REGION) ./scripts/provision.sh -var="bucket_name=$(BUCKET_NAME)"

build:
	@[ -n "$(PROJECT_ID)" ] || (echo "Set PROJECT_ID" && exit 1)
	PROJECT_ID=$(PROJECT_ID) REGION=$(REGION) ./scripts/build_push.sh

build-migrate:
	@[ -n "$(PROJECT_ID)" ] || (echo "Set PROJECT_ID" && exit 1)
	PROJECT_ID=$(PROJECT_ID) REGION=$(REGION) ./scripts/build_migrate_image.sh

deploy:
	@[ -n "$(PROJECT_ID)" ] || (echo "Set PROJECT_ID" && exit 1)
	@[ -n "$(BUCKET_NAME)" ] || (echo "Set BUCKET_NAME" && exit 1)
	@[ -n "$(IMAGE)" ] || (echo "Set IMAGE (from build)" && exit 1)
	PROJECT_ID=$(PROJECT_ID) REGION=$(REGION) IMAGE=$(IMAGE) BUCKET_NAME=$(BUCKET_NAME) MIGRATE_IMAGE=$(MIGRATE_IMAGE) ./scripts/deploy.sh

run-jobs:
	@[ -n "$(PROJECT_ID)" ] || (echo "Set PROJECT_ID" && exit 1)
	PROJECT_ID=$(PROJECT_ID) REGION=$(REGION) ./scripts/run_jobs.sh

outputs:
	terraform -chdir=infra output

test:
	@[ -n "$(PROJECT_ID)" ] || (echo "Set PROJECT_ID" && exit 1)
	BASE_URL=$$(terraform -chdir=infra output -raw cloud_run_url) ./scripts/test_k6.sh

destroy:
	@[ -n "$(PROJECT_ID)" ] || (echo "Set PROJECT_ID" && exit 1)
	PROJECT_ID=$(PROJECT_ID) REGION=$(REGION) ./scripts/destroy.sh

