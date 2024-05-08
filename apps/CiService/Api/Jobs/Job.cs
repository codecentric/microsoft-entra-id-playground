namespace CiService.Api.Jobs;

public record Job
{
    public Guid Id { get; init; }
    public string RepositoryName { get; init; } = "";
    public string ShellCommand { get; init; } = "";
    public DateTimeOffset CreatedTime { get; init; }
}
