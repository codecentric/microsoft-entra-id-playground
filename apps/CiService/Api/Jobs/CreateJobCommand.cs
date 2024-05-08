namespace CiService.Api.Jobs;

public record CreateJobCommand
{
    public string RepositoryName { get; init; } = "";
    public string ShellCommand { get; init; } = "";
}
