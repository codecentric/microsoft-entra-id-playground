provider "azurerm" {
  tenant_id = var.azure_tenant_id
  features {}
}

provider "azuread" {
  tenant_id = var.azure_tenant_id
}
