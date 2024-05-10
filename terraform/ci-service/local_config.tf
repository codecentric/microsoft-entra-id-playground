module "appsettings_compose_json" {
  source                    = "./appsettings"
  client_id                 = azuread_application_registration.ci_service.client_id
  client_secret             = azuread_service_principal_password.ci_service.value
  code_repository_base_url  = "http://code-repository-service:8080"
  code_repository_client_id = var.repository_service_client_id
}

resource "local_sensitive_file" "appsettings_compose_json" {
  filename        = "${path.root}/../apps/CiService/appsettings.Compose.json"
  file_permission = "0600"
  content         = module.appsettings_compose_json.appsettings_json
}

module "appsettings_development_json" {
  source                    = "./appsettings"
  client_id                 = azuread_application_registration.ci_service.client_id
  client_secret             = azuread_service_principal_password.ci_service.value
  code_repository_base_url  = "http://localhost:8082"
  code_repository_client_id = var.repository_service_client_id
}

resource "local_sensitive_file" "appsettings_development_json" {
  filename        = "${path.root}/../apps/CiService/appsettings.Development.json"
  file_permission = "0600"
  content         = module.appsettings_development_json.appsettings_json
}
