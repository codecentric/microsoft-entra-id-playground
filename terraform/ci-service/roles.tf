module "read_all_role" {
  source           = "../app_role"
  role_name        = "Read All Data"
  scope            = "Read.All"
  role_description = "Enables a service principal to read all data of this application."
  application_id           = azuread_application_registration.ci_service.id
}

module "read_write_all_role" {
  source           = "../app_role"
  role_name        = "Read and Write All Data"
  scope            = "ReadWrite.All"
  role_description = "Enables a service principal to read and write all data of this application."
  application_id           = azuread_application_registration.ci_service.id
}

module "job_read_all_role" {
  source           = "../app_role"
  role_name        = "Read All Job Data"
  scope            = "Jobs.Read.All"
  role_description = "Enables a service principal to read all job data of this application."
  application_id           = azuread_application_registration.ci_service.id
}

module "job_read_write_all_role" {
  source           = "../app_role"
  role_name        = "Read and Write All Job Data"
  scope            = "Jobs.ReadWrite.All"
  role_description = "Enables a service principal to read and write all job data of this application."
  application_id           = azuread_application_registration.ci_service.id
}
