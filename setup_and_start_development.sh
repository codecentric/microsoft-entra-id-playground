#!/usr/bin/env bash

set -ex

SCRIPT_DIR=$(dirname -- "$( readlink -f -- "$0"; )";)

"${SCRIPT_DIR}/setup_and_start.sh" swagger-ui
