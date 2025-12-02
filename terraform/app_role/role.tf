resource "random_uuid" "role_id" {}

resource "azuread_application_app_role" "role" {
  application_id       = var.application_id
  role_id              = random_uuid.role_id.id
  display_name         = var.role_name
  description          = var.role_description
  value                = var.scope
  allowed_member_types = [
    "Application",
    "User"
  ]
}
