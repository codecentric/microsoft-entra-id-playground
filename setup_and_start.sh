#!/usr/bin/env bash

SCRIPT_DIR=$(dirname -- "$( readlink -f -- "$0"; )";)

set -ex

"${SCRIPT_DIR}/terraform_apply.sh"

cd "${SCRIPT_DIR}/apps"

docker compose up --build --force-recreate $@
