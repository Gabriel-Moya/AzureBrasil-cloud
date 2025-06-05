using AzureBrasil_cloud.Application.Interfaces;
using AzureBrasil_cloud.Domain.Entities;

namespace AzureBrasil_cloud.Application.Services;

public class GroupService
{
    private readonly IAzureGraphService _graphService;

    public GroupService(IAzureGraphService graphService)
    {
        _graphService = graphService;
    }

    public Task<IEnumerable<Group>> GetGroupsAsync(string token)
        => _graphService.GetGroupsAsync(token);
}
