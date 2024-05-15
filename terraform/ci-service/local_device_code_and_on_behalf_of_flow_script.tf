# We need to allow public client flows to be able to use the device code flow
resource "azuread_application_fallback_public_client" "ci_service" {
  application_id = azuread_application_registration.ci_service.id
  enabled        = true
}

resource "local_sensitive_file" "ci_service_device_code_and_on_behalf_of_flow_for_code_repository_service_script" {
  filename        = "${path.root}/../ci-service.device-code.and.on-behalf-of.flow.for.code-repository-service.sh"
  file_permission = "0700"
  content         = <<-EOF
#!/usr/bin/env bash
set -e

DEVICE_CODE_CHALLENGE=$(curl --fail --silent https://login.microsoftonline.com/${data.azuread_client_config.current.tenant_id}/oauth2/v2.0/devicecode \
  -d 'client_id=${azuread_application_registration.ci_service.client_id}' \
  -d 'scope=${azuread_application_registration.ci_service.client_id}/.default')

echo "Device Code Challenge:"
echo "$DEVICE_CODE_CHALLENGE" | jq
echo

MESSAGE=$(echo "$DEVICE_CODE_CHALLENGE" | jq -r .message)
DEVICE_CODE=$(echo "$DEVICE_CODE_CHALLENGE" | jq -r .device_code)
POLL_INTERVAL=$(echo "$DEVICE_CODE_CHALLENGE" | jq -r .interval)

echo "$MESSAGE"

set +e

DEVICE_CODE_TOKEN_RESPONSE=''
while [ -z "$DEVICE_CODE_TOKEN_RESPONSE" ]; do
  sleep $POLL_INTERVAL
  
  DEVICE_CODE_TOKEN_RESPONSE=$(curl --fail --silent https://login.microsoftonline.com/${data.azuread_client_config.current.tenant_id}/oauth2/v2.0/token \
    -d 'grant_type=urn:ietf:params:oauth:grant-type:device_code' \
    -d 'client_id=${azuread_application_registration.ci_service.client_id}' \
    -d "device_code=$DEVICE_CODE")
done

echo "Device Code Flow Response:"
echo "$DEVICE_CODE_TOKEN_RESPONSE" | jq

echo
echo

CI_SERVICE_TOKEN=$(echo "$DEVICE_CODE_TOKEN_RESPONSE" | jq -r .access_token)

echo "On-Behalf-Of Flow Response:"
curl --silent https://login.microsoftonline.com/${data.azuread_client_config.current.tenant_id}/oauth2/v2.0/token \
  -d 'grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer' \
  -d 'requested_token_use=on_behalf_of' \
  -d 'scope=${var.repository_service_client_id}/.default' \
  -d 'client_id=${azuread_application_registration.ci_service.client_id}' \
  -d 'client_secret=${urlencode(azuread_service_principal_password.ci_service.value)}' \
  -d "assertion=$CI_SERVICE_TOKEN" | jq
EOF
}
