#!/usr/bin/env bash

set -e

SCRIPT_DIR=$(dirname -- "$( readlink -f -- "$0"; )";)

. "${SCRIPT_DIR}/config.sh" || ( echo "config.sh is missing in root directory of this repository! Create it and export missing environment variables on subsequent errors. See config.example.sh as a reference." && false )

set -ex

cd "${SCRIPT_DIR}/apps"

docker compose down -v

cd "${SCRIPT_DIR}/terraform"

terraform destroy -input=false -auto-approve
