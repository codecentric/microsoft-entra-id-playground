resource "random_uuid" "scope_id" {}

resource "azuread_application_permission_scope" "scope" {
  application_id             = var.application_id
  scope_id                   = random_uuid.scope_id.id
  admin_consent_description  = var.scope_description
  admin_consent_display_name = var.scope_name
  user_consent_description   = var.scope_description
  user_consent_display_name  = var.scope_name
  value                      = var.scope
}
