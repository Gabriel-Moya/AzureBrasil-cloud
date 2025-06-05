using AzureBrasil_cloud.Domain.Entities;

namespace AzureBrasil_cloud.Application.Interfaces;

public interface IAzureGraphService
{
    Task<User> GetCurrentUserAsync(string accessToken);
    Task<Tenant> GetTenantAsync(string accessToken);
    Task<IEnumerable<User>> GetUsersAsync(string accessToken);
    Task<IEnumerable<Group>> GetGroupsAsync(string accessToken);
    Task<IEnumerable<LoginAttempt>> GetLoginAttemptsAsync(string accessToken, int skip, int top);
}
