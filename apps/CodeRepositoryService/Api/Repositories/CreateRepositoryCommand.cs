namespace CodeRepositoryService.Api.Repositories;

public record CreateRepositoryCommand
{
    public string RepositoryName { get; init; } = "";
}
