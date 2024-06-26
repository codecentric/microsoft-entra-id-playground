output "client_id" {
  value = azuread_application_registration.code_repository_service.client_id
}

output "principal_id" {
  value = azuread_service_principal.code_repository_service.object_id
}

output "code_read_all_scope_id" {
  value = module.code_read_all_scope.id
}

output "code_read_all_role_id" {
  value = module.code_read_all_role.id
}
