module "code_repository_service" {
  source = "./code-repository-service"
  app_name_prefix = var.azuread_app_names_prefix
}

module "ci_service" {
  source = "./ci-service"
  app_name_prefix = var.azuread_app_names_prefix
  repository_service_client_id = module.code_repository_service.client_id
  repository_service_code_read_all_scope_id = module.code_repository_service.code_read_all_scope_id
  repository_service_code_read_all_role_id = module.code_repository_service.code_read_all_role_id
}
