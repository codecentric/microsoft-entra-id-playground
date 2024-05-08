using CiService.DownstreamApi.CodeRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace CiService.Api.Jobs;

[Authorize]
[ApiController]
[Route("[controller]")]
public class JobsController : ControllerBase
{
    private const string ReadAllScope = $"Jobs.{GlobalScopes.ReadAll}";
    private const string ReadWriteAllScope = $"Jobs.{GlobalScopes.ReadWriteAll}";
    public const string UserReadAllScope = $"{GlobalScopes.UserImpersonationPrefix}.{ReadAllScope}";
    public const string UserReadWriteAllScope = $"{GlobalScopes.UserImpersonationPrefix}.{ReadWriteAllScope}";

    private readonly InMemoryJobRepository _inMemoryJobRepository;
    private readonly ILogger<JobsController> _logger;

    public JobsController(InMemoryJobRepository inMemoryJobRepository, ILogger<JobsController> logger)
    {
        _inMemoryJobRepository = inMemoryJobRepository;
        _logger = logger;
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
    [HttpPost(Name = "CreateJob")]
    public async Task<ActionResult<Job>> Create(
        [FromBody] CreateJobCommand command,
        [FromQuery] bool impersonateUser,
        CodeRepositoryApi codeRepositoryApi,
        CancellationToken cancellationToken)
    {
        var securityPrincipalObjectId = User.GetObjectId();

        if (string.IsNullOrWhiteSpace(securityPrincipalObjectId))
        {
            return new UnauthorizedResult();
        }

        var result = await codeRepositoryApi.GetCode(command.RepositoryName, impersonateUser, cancellationToken);

        _logger.LogInformation($"Executing command '{command.ShellCommand}' for repository code: {result?.Content}");

        var job = _inMemoryJobRepository.CreateJob(securityPrincipalObjectId, command);

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
    [HttpGet(Name = "GetJobs")]
    public ActionResult<IEnumerable<Job>> GetAll()
    {
        var securityPrincipalObjectId = User.GetObjectId();

        if (string.IsNullOrWhiteSpace(securityPrincipalObjectId))
        {
            return new UnauthorizedResult();
        }

        return new JsonResult(_inMemoryJobRepository.GetAllByUserId(securityPrincipalObjectId));
    }

    [RequiredScopeOrAppPermission(
        AcceptedAppPermission =
        [
            GlobalScopes.ReadAll,
            GlobalScopes.ReadWriteAll,
            ReadAllScope,
            ReadWriteAllScope
        ],
        AcceptedScope =
        [
            GlobalScopes.UserReadAll,
            GlobalScopes.UserReadWriteAll,
            UserReadAllScope,
            UserReadWriteAllScope
        ])
    ]
    [HttpGet(template: "{jobId}", Name = "GetJob")]
    public ActionResult<IEnumerable<Job>> Get(Guid jobId)
    {
        var securityPrincipalObjectId = User.GetObjectId();

        if (string.IsNullOrWhiteSpace(securityPrincipalObjectId))
        {
            return new UnauthorizedResult();
        }

        return new JsonResult(_inMemoryJobRepository.Get(jobId, securityPrincipalObjectId));
    }
}