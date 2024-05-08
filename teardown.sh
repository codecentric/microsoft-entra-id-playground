#!/usr/bin/env bash

set -ex

SCRIPT_DIR=$(dirname -- "$( readlink -f -- "$0"; )";)

. "${SCRIPT_DIR}/config.sh"

cd "${SCRIPT_DIR}/apps"

docker compose down -v

"${SCRIPT_DIR}/terraform/destroy.sh"
