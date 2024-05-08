namespace CiService;

public static class ConfigurationKeys
{
    public const string CiService = nameof(CiService);
    public const string ApplicationScopes = $"{CiService}:{nameof(ApplicationScopes)}";
}