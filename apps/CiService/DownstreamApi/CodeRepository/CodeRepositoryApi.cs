using System.Net;
using Microsoft.Identity.Abstractions;
using Newtonsoft.Json;

namespace CiService.DownstreamApi.CodeRepository;

public class CodeRepositoryApi
{
    private readonly IDownstreamApi _downstreamApi;
    private readonly ILogger<CodeRepositoryApi> _logger;
    public const string DownstreamApiName = nameof(CodeRepositoryApi);

    public CodeRepositoryApi(IDownstreamApi downstreamApi, ILogger<CodeRepositoryApi> logger)
    {
        _downstreamApi = downstreamApi;
        _logger = logger;
    }

    public async Task<Code?> GetCode(string repositoryName, bool impersonateUser, CancellationToken cancellationToken = default)
    {
        var response = await _downstreamApi.CallApiAsync(
            DownstreamApiName,
            options =>
            {
                options.HttpMethod = HttpMethod.Get.Method;
                options.RelativePath = $"repositories/{Uri.EscapeDataString(repositoryName)}/code";
                options.RequestAppToken = !impersonateUser;
            },
            cancellationToken: cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning($"{response.RequestMessage?.Method.Method} {response.RequestMessage?.RequestUri} returned status code {response.StatusCode}!");
        }

        return JsonConvert.DeserializeObject<Code>(await response.Content.ReadAsStringAsync(cancellationToken));
    }
}