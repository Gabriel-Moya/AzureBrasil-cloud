using AzureBrasil_cloud.Application.Interfaces;
using AzureBrasil_cloud.Domain.Entities;

public class UserService
{
    private readonly IAzureGraphService _graphService;

    public UserService(IAzureGraphService graphService)
    {
        _graphService = graphService;
    }

    public Task<User> GetCurrentUserAsync(string token) => _graphService.GetCurrentUserAsync(token);
    public Task<IEnumerable<User>> GetUsersAsync(string token) => _graphService.GetUsersAsync(token);
}
