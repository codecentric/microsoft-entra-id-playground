# Entra ID service to service communication example

This repository provides an example how to implement service to service communication between custom services.

## How to execute the example

To bootstrap the example against a custom Azure tenant, just execute the `setup_and_start.sh` in the root directory.
It will print error messages if missing configuration is detected.

Executing `setup_and_start.sh` sucessfully will start a local swagger-ui, available on http://localhost:8080.

Example workflow for on-behalf-of flow:

* Login as a user on the code repository api.
* Create a Repository
* Update the code of the repository
* Login as a user on the ci repository api
  * Choose all available scopes
* Create a job for the repository of the same user
  * Enable user impersonation parameter
