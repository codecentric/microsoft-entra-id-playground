using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace CodeRepositoryService.Api.Repositories.CodeApi;

[Authorize]
[ApiController]
[Route($"{RepositoriesController.RepositoryRoute}/{{repositoryName}}/[controller]")]
public class CodeController : ControllerBase
{
    public const string ReadAllScope = $"{RepositoriesController.RepositoriesScope}.Code.{GlobalScopes.ReadAll}";

    public const string ReadWriteAllScope =
        $"{RepositoriesController.RepositoriesScope}.Code.{GlobalScopes.ReadWriteAll}";

    public const string UserReadAllScope = $"{GlobalScopes.UserImpersonationPrefix}.{ReadAllScope}";
    public const string UserReadWriteAllScope = $"{GlobalScopes.UserImpersonationPrefix}.{ReadWriteAllScope}";

    private readonly InMemoryCodeRepository _inMemoryCodeRepository;
    private readonly ILogger<CodeController> _logger;

    public CodeController(InMemoryCodeRepository inMemoryCodeRepository)
    {
        _inMemoryCodeRepository = inMemoryCodeRepository;
    }

    [RequiredScopeOrAppPermission(
        AcceptedAppPermission =
        [
            GlobalScopes.ReadWriteAll,
            RepositoriesController.ReadWriteAllScope,
            ReadWriteAllScope,
        ],
        AcceptedScope =
        [
            GlobalScopes.UserReadWriteAll,
            RepositoriesController.UserReadWriteAllScope,
            UserReadWriteAllScope
        ])]
    [HttpPut(Name = "UpdateCode")]
    public async Task<ActionResult> Put([FromBody] Code code, string repositoryName)
    {
        _inMemoryCodeRepository.UpdateCode(code, repositoryName, User);

        return new OkResult();
    }

    [RequiredScopeOrAppPermission(
        AcceptedAppPermission =
        [
            GlobalScopes.ReadAll,
            GlobalScopes.ReadWriteAll,
            RepositoriesController.ReadAllScope,
            RepositoriesController.ReadWriteAllScope,
            ReadAllScope,
            ReadWriteAllScope,
        ],
        AcceptedScope =
        [
            GlobalScopes.UserReadAll,
            GlobalScopes.UserReadWriteAll,
            RepositoriesController.UserReadAllScope,
            RepositoriesController.UserReadWriteAllScope,
            UserReadAllScope,
            UserReadWriteAllScope
        ])]
    [HttpGet]
    public ActionResult<IEnumerable<Code>> Get(string repositoryName)
    {
        return new JsonResult(_inMemoryCodeRepository.Get(repositoryName, User));
    }
}