#!/usr/bin/env bash

SCRIPT_DIR=$(dirname -- "$( readlink -f -- "$0"; )";)

set -ex

"${SCRIPT_DIR}/setup_and_start.sh" swagger-ui
