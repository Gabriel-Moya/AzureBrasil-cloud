using AzureBrasil_cloud.Application.Interfaces;
using AzureBrasil_cloud.Domain.Entities;

namespace AzureBrasil_cloud.Application.Services;

public class LoginAttemptService
{
    private readonly IAzureGraphService _graphService;

    public LoginAttemptService(IAzureGraphService graphService)
    {
        _graphService = graphService;
    }

    public async Task<IEnumerable<LoginAttempt>> GetLoginAttemptsAsync(
        string token,
        int skip,
        int top
    )
        => await _graphService.GetLoginAttemptsAsync(token, skip, top);
}
