module "read_all_scope" {
  source            = "../scope"
  scope_name        = "Read all data on behalf of a user"
  scope             = "UserImpersonation.Read.All"
  scope_description = "Enables a service principal to read all data on behalf of a user."
  application_id    = azuread_application_registration.code_repository_service.id
}

module "read_write_all_scope" {
  source            = "../scope"
  scope_name        = "Read and write all data on behalf of a user"
  scope             = "UserImpersonation.ReadWrite.All"
  scope_description = "Enables a service principal to read and write all data on behalf of a user."
  application_id    = azuread_application_registration.code_repository_service.id
}

module "repository_read_all_scope" {
  source            = "../scope"
  scope_name        = "Read all code data on behalf of a user"
  scope             = "UserImpersonation.Repositories.Read.All"
  scope_description = "Enables a service principal to read all repository data on behalf of a user."
  application_id    = azuread_application_registration.code_repository_service.id
}

module "repository_read_write_all_scope" {
  source            = "../scope"
  scope_name        = "Read and write all code data on behalf of a user"
  scope             = "UserImpersonation.Repositories.ReadWrite.All"
  scope_description = "Enables a service principal to read and write all repository data on behalf of a user."
  application_id    = azuread_application_registration.code_repository_service.id
}

module "code_read_all_scope" {
  source            = "../scope"
  scope_name        = "Read all code data on behalf of a user"
  scope             = "UserImpersonation.Repositories.Code.Read.All"
  scope_description = "Enables a service principal to read all code data on behalf of a user."
  application_id    = azuread_application_registration.code_repository_service.id
}

module "code_read_write_all_scope" {
  source            = "../scope"
  scope_name        = "Read and write all code data on behalf of a user"
  scope             = "UserImpersonation.Repositories.Code.ReadWrite.All"
  scope_description = "Enables a service principal to read and write all code data on behalf of a user."
  application_id    = azuread_application_registration.code_repository_service.id
}
