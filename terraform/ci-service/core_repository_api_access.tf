resource "azuread_application_api_access" "code_repository" {
  application_id = azuread_application_registration.ci_service.id
  api_client_id  = var.repository_service_client_id
  scope_ids = [
    var.repository_service_code_read_all_scope_id
  ]
  role_ids = [
    var.repository_service_code_read_all_role_id
  ]
}
