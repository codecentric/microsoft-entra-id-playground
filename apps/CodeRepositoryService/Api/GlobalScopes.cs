namespace CodeRepositoryService.Api;

public class GlobalScopes
{
    public const string ReadAll = "Read.All";
    public const string ReadWriteAll = "ReadWrite.All";
    public const string UserImpersonationPrefix = "UserImpersonation";
    public const string UserReadAll = $"{UserImpersonationPrefix}.{ReadAll}";
    public const string UserReadWriteAll = $"{UserImpersonationPrefix}.{ReadWriteAll}";
}
