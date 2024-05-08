using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace CodeRepositoryService.Api.Repositories;

[Authorize]
[ApiController]
[Route(RepositoryRoute)]
public class RepositoriesController : ControllerBase
{
    public const string RepositoryRoute = "repositories";

    public const string RepositoriesScope = "Repositories";
    public const string ReadAllScope = $"{RepositoriesScope}.{GlobalScopes.ReadAll}";
    public const string ReadWriteAllScope = $"{RepositoriesScope}.{GlobalScopes.ReadWriteAll}";
    public const string UserReadAllScope = $"{GlobalScopes.UserImpersonationPrefix}.{ReadAllScope}";
    public const string UserReadWriteAllScope = $"{GlobalScopes.UserImpersonationPrefix}.{ReadWriteAllScope}";

    private readonly InMemoryRepositoryRepository _inMemoryRepositoryRepository;

    public RepositoriesController(InMemoryRepositoryRepository inMemoryRepositoryRepository)
    {
        _inMemoryRepositoryRepository = inMemoryRepositoryRepository;
    }

    [RequiredScopeOrAppPermission(
        AcceptedAppPermission =
        [
            GlobalScopes.ReadWriteAll,
            ReadWriteAllScope,
        ],
        AcceptedScope =
        [
            GlobalScopes.UserReadWriteAll,
            UserReadWriteAllScope
        ])]
    [HttpPost(Name = "CreateRepository")]
    public async Task<ActionResult<Repository>> Create([FromBody] CreateRepositoryCommand command)
    {
        var securityPrincipalObjectId = User.GetObjectId();

        if (string.IsNullOrWhiteSpace(securityPrincipalObjectId))
        {
            return new UnauthorizedResult();
        }

        var job = _inMemoryRepositoryRepository.CreateRepository(securityPrincipalObjectId, command);

        return new JsonResult(job);
    }

    [RequiredScopeOrAppPermission(
        AcceptedAppPermission =
        [
            GlobalScopes.ReadAll,
            GlobalScopes.ReadWriteAll,
            ReadAllScope,
            ReadWriteAllScope,
        ],
        AcceptedScope =
        [
            GlobalScopes.UserReadAll,
            GlobalScopes.UserReadWriteAll,
            UserReadAllScope,
            UserReadWriteAllScope
        ])]
    [HttpGet(Name = "GetRepositories")]
    public ActionResult<IEnumerable<Repository>> GetAll()
    {
        var securityPrincipalObjectId = User.GetObjectId();

        if (string.IsNullOrWhiteSpace(securityPrincipalObjectId))
        {
            return new UnauthorizedResult();
        }

        return new JsonResult(_inMemoryRepositoryRepository.GetAllByUserId(securityPrincipalObjectId));
    }

    [RequiredScopeOrAppPermission(
        AcceptedAppPermission =
        [
            GlobalScopes.ReadAll,
            GlobalScopes.ReadWriteAll,
            ReadAllScope,
            ReadWriteAllScope,
        ],
        AcceptedScope =
        [
            GlobalScopes.UserReadAll,
            GlobalScopes.UserReadWriteAll,
            UserReadAllScope,
            UserReadWriteAllScope
        ])]
    [HttpGet(template: "{repositoryName}", Name = "GetRepository")]
    public ActionResult<IEnumerable<Repository>> Get(string repositoryName)
    {
        return new JsonResult(_inMemoryRepositoryRepository.Get(repositoryName, User));
    }
}