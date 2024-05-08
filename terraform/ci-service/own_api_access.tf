resource "azuread_application_api_access" "own" {
  application_id = azuread_application_registration.ci_service.id
  api_client_id  = azuread_application_registration.ci_service.client_id
  scope_ids = [
    module.read_write_all_scope.id
  ]
}
