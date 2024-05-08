using System.Collections.Immutable;
using System.Security.Claims;
using CodeRepositoryService.Api.Repositories.CodeApi;
using CodeRepositoryService.Extensions;
using Microsoft.Identity.Web;

namespace CodeRepositoryService.Api.Repositories;

public class InMemoryRepositoryRepository
{
    private readonly Dictionary<string, ImmutableHashSet<Repository>> _repositoriesByUserId = new();
    private readonly Dictionary<string, Repository> _repositoriesByRepositoryName = new();

    public Repository CreateRepository(string userId, CreateRepositoryCommand command)
    {
        var repository = new Repository()
        {
            InternalId = Guid.NewGuid(),
            RepositoryName = command.RepositoryName,
            CreatedTime = DateTimeOffset.UtcNow
        };
        _repositoriesByUserId.TryGetValue(userId, out var userRepositories);
        _repositoriesByUserId[userId] = (userRepositories ?? ImmutableHashSet<Repository>.Empty).Add(repository);

        _repositoriesByRepositoryName[repository.RepositoryName] = repository;

        return repository;
    }

    public IReadOnlySet<Repository> GetAllByUserId(string userId)
    {
        _repositoriesByUserId.TryGetValue(userId, out var userRepositories);
        return userRepositories ?? ImmutableHashSet<Repository>.Empty;
    }

    public Repository? Get(string repositoryName, ClaimsPrincipal principal)
    {
        var hasAppRole = principal.IsInAnyRole(
            GlobalScopes.ReadAll,
            GlobalScopes.ReadWriteAll,
            RepositoriesController.ReadAllScope,
            RepositoriesController.ReadWriteAllScope,
            CodeController.ReadAllScope,
            CodeController.ReadWriteAllScope);

        var hasUserImpersonationScope = principal.HasAnyScope(
            GlobalScopes.UserReadAll,
            GlobalScopes.UserReadWriteAll,
            RepositoriesController.UserReadAllScope,
            RepositoriesController.UserReadWriteAllScope,
            CodeController.UserReadAllScope,
            CodeController.UserReadWriteAllScope
        );

        var needsUserImpersonation = !hasAppRole;

        ImmutableHashSet<Repository>? userRepositories = null;

        if (hasUserImpersonationScope && !_repositoriesByUserId.TryGetValue(principal.GetObjectId()!, out userRepositories))
        {
            return null;
        }

        _repositoriesByRepositoryName.TryGetValue(repositoryName, out var repository);

        if (repository == null)
        {
            return null;
        }

        if (needsUserImpersonation && (userRepositories == null || !userRepositories.Contains(repository)))
        {
            return null;
        }

        return repository;
    }
}