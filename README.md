# Entra ID service to service communication example

This repository provides an example how to implement service to service communication between custom services.

## How to execute the example

1. Create a `config.sh` file, similar to the `config.example.sh`.
2. Execute the `setup_and_start.sh` in the root directory.
  * Provisions example App registrations via terraform in target Entra ID directory
  * Creates required `appsettings.Compose.json` in `apps/CiService` and `apps/CodeRepositoryService`
  * Starts a swagger-ui on http://localhost:8080
  * Builds and starts CiService (port 8081) and CodeRepositoryService (port 8082) as containers

## Example scenario

Example workflow for on-behalf-of flow:

* Login as a user on the code repository api.
* Create a Repository
* Update the code of the repository
* Login as a user on the ci repository api
  * Choose all available scopes
* Create a job for the repository of the same user
  * Enable user impersonation parameter
