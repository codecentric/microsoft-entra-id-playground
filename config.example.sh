#!/usr/bin/env bash

# All azure ad apps will be prefixed with this string. Can be used as a namespace for those apps.
export TF_VAR_azuread_app_names_prefix="service-to-service-example"
export TF_VAR_azure_tenant_id="__YOUR_AZURE_TENANT_ID__"
