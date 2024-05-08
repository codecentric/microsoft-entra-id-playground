terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.99.0"
    }
    azuread = {
      source  = "hashicorp/azuread"
      version = "~> 2.48.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 3.6.1"
    }
  }

  required_version = "~> 1.8"
}
