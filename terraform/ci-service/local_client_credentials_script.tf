resource "local_sensitive_file" "ci_service_client_credentials_for_code_repository_service_script" {
  filename        = "${path.root}/../ci-service.client_credentials.for.code-repository-service.sh"
  file_permission = "0700"
  content         = <<-EOF
#!/usr/bin/env bash
set -ex
curl https://login.microsoftonline.com/${data.azuread_client_config.current.tenant_id}/oauth2/v2.0/token \
  -d 'grant_type=client_credentials' \
  -d 'scope=${var.repository_service_client_id}/.default' \
  -d 'client_id=${azuread_application_registration.ci_service.client_id}' \
  -d 'client_secret=${urlencode(azuread_service_principal_password.ci_service.value)}'

EOF
}
