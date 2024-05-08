#!/usr/bin/env bash

SCRIPT_DIR=$(dirname -- "$( readlink -f -- "$0"; )";)

cd "$SCRIPT_DIR"

set -ex

test -n "${TF_VAR_azuread_app_names_prefix}" || ( echo 'TF_VAR_azuread_app_names_prefix is not defined!' && false )
test -n "${TF_VAR_azure_tenant_id}" || ( echo 'TF_VAR_azure_tenant_id is not defined!' && false )

terraform init
terraform apply -auto-approve
