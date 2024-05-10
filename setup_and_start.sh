#!/usr/bin/env bash

set -e

SCRIPT_DIR=$(dirname -- "$( readlink -f -- "$0"; )";)

. "${SCRIPT_DIR}/config.sh" || ( echo "config.sh is missing in root directory of this repository! Create it and export missing environment variables on subsequent errors. See config.example.sh as a reference." && false )

set -ex

"${SCRIPT_DIR}/terraform/apply.sh"

cd "${SCRIPT_DIR}/apps"

docker compose up --build --force-recreate $@
