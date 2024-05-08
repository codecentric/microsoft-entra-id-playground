resource "azuread_application_registration" "code_repository_service" {
  display_name = "${var.app_name_prefix}-code-repository-service"
}

data "azuread_client_config" "current" {}

resource "azuread_application_owner" "ci_service" {
  application_id  = azuread_application_registration.code_repository_service.id
  owner_object_id = data.azuread_client_config.current.object_id
}

resource "azuread_application_redirect_uris" "ci_service" {
  application_id = azuread_application_registration.code_repository_service.id
  redirect_uris  = [
    # local swagger redirect uri. Should not be set in a production environment
    "http://localhost:8080/oauth2-redirect.html"
  ]
  type = "SPA"
}

resource "azuread_application_identifier_uri" "code_repository_uri" {
  application_id = azuread_application_registration.code_repository_service.id
  identifier_uri = "api://${azuread_application_registration.code_repository_service.client_id}"
}

resource "azuread_service_principal" "ci_service" {
  client_id = azuread_application_registration.code_repository_service.client_id
  account_enabled = true
  app_role_assignment_required = false
}
