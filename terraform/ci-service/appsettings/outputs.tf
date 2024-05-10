locals {
  code_repsitory_default_scope = "${var.code_repository_client_id}/.default"
}

output "appsettings_json" {
  value = <<-EOF
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": ${jsonencode(data.azuread_client_config.current.tenant_id)},
    "ClientId": ${jsonencode(var.client_id)},
    "ClientSecret": ${jsonencode(var.client_secret)}
  },
  "DownstreamApis": {
    "CodeRepositoryApi": {
      "BaseUrl": ${jsonencode(var.code_repository_base_url)},
      "Scopes": [${jsonencode(local.code_repsitory_default_scope)}]
    }
  },
  "Cors": {
    "DockerComposeSwaggerUi": {
      "Origins": [
        "http://localhost:8080"
      ],
      "Methods": ["GET", "POST", "PUT", "DELETE", "PATCH"],
      "Headers": ["Authorization", "Content-Type"],
      "SupportsCredentials": true
    }
  },
  "Logging": {
    "LogLevel": {
      "CiService.Middleware.LogAuthorizationHeaderMiddleware": "Trace",
      "Default": "Information",
      "Microsoft.AspNetCore.DataProtection": "Error"
    }
  },
  "AllowedHosts": "*"
}
EOF
}