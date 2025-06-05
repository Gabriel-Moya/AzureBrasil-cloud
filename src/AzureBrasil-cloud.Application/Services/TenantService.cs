using AzureBrasil_cloud.Application.Interfaces;
using AzureBrasil_cloud.Domain.Entities;

namespace AzureBrasil_cloud.Application.Services;

public class TenantService
{
    private readonly IAzureGraphService _graphService;

    public TenantService(IAzureGraphService graphService)
    {
        _graphService = graphService;
    }

    public Task<Tenant> GetTenantAsync(string token)
        => _graphService.GetTenantAsync(token);
}
