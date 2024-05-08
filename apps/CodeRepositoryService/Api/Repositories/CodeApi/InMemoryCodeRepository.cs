using System.Security.Claims;

namespace CodeRepositoryService.Api.Repositories.CodeApi;

public class InMemoryCodeRepository
{
    private readonly InMemoryRepositoryRepository _inMemoryRepositoryRepository;
    private readonly Dictionary<Guid, Code> _codeByRepositoryId = new();

    public InMemoryCodeRepository(InMemoryRepositoryRepository inMemoryRepositoryRepository)
    {
        _inMemoryRepositoryRepository = inMemoryRepositoryRepository;
    }

    public void UpdateCode(Code code, string repositoryName, ClaimsPrincipal claimsPrincipal)
    {
        var repository = GetRepository(repositoryName, claimsPrincipal);

        _codeByRepositoryId[repository.InternalId] = code;
    }

    public Code? Get(string repositoryName, ClaimsPrincipal claimsPrincipal)
    {
        var repository = GetRepository(repositoryName, claimsPrincipal);

        _codeByRepositoryId.TryGetValue(repository.InternalId, out var code);

        return code;
    }

    private Repository GetRepository(string repositoryName, ClaimsPrincipal claimsPrincipal)
    {
        var repository = _inMemoryRepositoryRepository.Get(repositoryName, claimsPrincipal);

        if (repository == null)
        {
            throw new InvalidOperationException("Repository does not exist!");
        }

        return repository;
    }
}