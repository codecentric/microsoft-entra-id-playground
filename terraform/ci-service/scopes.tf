module "read_all_scope" {
  source            = "../scope"
  scope_name        = "Read all data on behalf of a user"
  scope             = "UserImpersonation.Read.All"
  scope_description = "Enables a service principal to read all data on behalf of a user."
  application_id    = azuread_application_registration.ci_service.id
}

module "read_write_all_scope" {
  source            = "../scope"
  scope_name        = "Read and write all data on behalf of a user"
  scope             = "UserImpersonation.ReadWrite.All"
  scope_description = "Enables a service principal to read and write all data on behalf of a user."
  application_id    = azuread_application_registration.ci_service.id
}

module "job_read_all_scope" {
  source            = "../scope"
  scope_name        = "Read all job data on behalf of a user"
  scope             = "UserImpersonation.Jobs.Read.All"
  scope_description = "Enables a service principal to read all job data on behalf of a user."
  application_id    = azuread_application_registration.ci_service.id
}

module "job_read_write_all_scope" {
  source            = "../scope"
  scope_name        = "Read and write all job data on behalf of a user"
  scope             = "UserImpersonation.Jobs.ReadWrite.All"
  scope_description = "Enables a service principal to read and write all job data on behalf of a user."
  application_id    = azuread_application_registration.ci_service.id
}
