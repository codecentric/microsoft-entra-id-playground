using System.Text.Json.Serialization;

namespace CodeRepositoryService.Api.Repositories;

public record Repository
{
    [JsonIgnore] public Guid InternalId { get; init; }
    public string RepositoryName { get; init; } = "";
    public DateTimeOffset CreatedTime { get; init; }
}