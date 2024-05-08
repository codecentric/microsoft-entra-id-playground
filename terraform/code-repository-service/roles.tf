module "read_all_role" {
  source = "../app_role"
  role_name = "Read All Data"
  scope = "Read.All"
  role_description = "Enables a service principal to read all data of this application."
  application_id = azuread_application_registration.code_repository_service.id
}

module "read_write_all_role" {
  source = "../app_role"
  role_name = "Read and Write All Data"
  scope = "ReadWrite.All"
  role_description = "Enables a service principal to read and write all data of this application."
  application_id = azuread_application_registration.code_repository_service.id
}

module "repository_read_all_role" {
  source = "../app_role"
  role_name = "Read All Repository Data"
  scope = "Repository.Read.All"
  role_description = "Enables a service principal to read all repository data of this application."
  application_id = azuread_application_registration.code_repository_service.id
}

module "repository_read_write_all_role" {
  source = "../app_role"
  role_name = "Read and Write All Repository Code Data"
  scope = "Repository.ReadWrite.All"
  role_description = "Enables a service principal to read and write all repository data of this application."
  application_id = azuread_application_registration.code_repository_service.id
}

module "code_read_all_role" {
  source = "../app_role"
  role_name = "Read All Repository Code Data"
  scope = "Repositories.Code.Read.All"
  role_description = "Enables a service principal to read all repository code data of this application."
  application_id = azuread_application_registration.code_repository_service.id
}

module "code_read_write_all_role" {
  source = "../app_role"
  role_name = "Read and Write All Repository Code Data"
  scope = "Repositories.Code.ReadWrite.All"
  role_description = "Enables a service principal to read and write all repository code data of this application."
  application_id = azuread_application_registration.code_repository_service.id
}
