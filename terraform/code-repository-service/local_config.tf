module "appsettings_compose_json" {
  source                    = "./appsettings"
  client_id                 = azuread_application_registration.code_repository_service.client_id
}

resource "local_sensitive_file" "appsettings_compose_json" {
  filename        = "${path.root}/../apps/CodeRepositoryService/appsettings.Compose.json"
  file_permission = "0600"
  content         = module.appsettings_compose_json.appsettings_json
}

module "appsettings_development_json" {
  source                    = "./appsettings"
  client_id                 = azuread_application_registration.code_repository_service.client_id
}

resource "local_sensitive_file" "appsettings_development_json" {
  filename        = "${path.root}/../apps/CodeRepositoryService/appsettings.Development.json"
  file_permission = "0600"
  content         = module.appsettings_development_json.appsettings_json
}
