using AzureBrasil_cloud.Application.Interfaces;
using AzureBrasil_cloud.Domain.Entities;
using Microsoft.Graph;
using Microsoft.Graph.AuditLogs.SignIns;
using Microsoft.Kiota.Abstractions.Authentication;
using System.Text.Json;

namespace AzureBrasil_cloud.Infra.ExternalServices.AzureGraphService;

public class AzureGraphService : IAzureGraphService
{
    public async Task<User> GetCurrentUserAsync(string accessToken)
    {
        var graphClient = GetGraphClient(accessToken);
        var me = await graphClient.Me.GetAsync();

        return new User
        {
            Id = me.Id,
            DisplayName = me.DisplayName,
            Email = me.Mail ?? me.UserPrincipalName,
        };
    }

    public async Task<Tenant> GetTenantAsync(string accessToken)
    {
        var graphClient = GetGraphClient(accessToken);
        var orgs = await graphClient.Organization.GetAsync();
        var org = orgs.Value.FirstOrDefault();

        return new Tenant
        {
            Id = org.Id,
            DisplayName = org.DisplayName
        };
    }

    public async Task<IEnumerable<User>> GetUsersAsync(string accessToken)
    {
        var graphClient = GetGraphClient(accessToken);
        var users = await graphClient.Users.GetAsync();
        return users.Value.Select(User => new User
        {
            Id = User.Id,
            DisplayName = User.DisplayName,
            Email = User.Mail ?? User.UserPrincipalName
        });
    }

    public async Task<IEnumerable<Group>> GetGroupsAsync(string accessToken)
    {
        var graphClient = GetGraphClient(accessToken);
        var groups = await graphClient.Groups.GetAsync();
        return groups.Value.Select(g => new Group
        {
            Id = g.Id,
            DisplayName = g.DisplayName,
            Description = g.Description
        });
    }

    public async Task<IEnumerable<LoginAttempt>> GetLoginAttemptsAsync(string accessToken, int skip, int top)
    {
        var graphClient = GetGraphClient(accessToken);
        var filter = "createdDateTime ge " + DateTime.UtcNow.AddDays(-3).ToString("o");

        var allSignIns = new List<Microsoft.Graph.Models.SignIn>();
        var resultPage = await graphClient.AuditLogs.SignIns.GetAsync(config =>
        {
            config.QueryParameters.Filter = filter;
            config.QueryParameters.Top = top;
        });

        int totalCount = 0;
        while (resultPage != null && totalCount < skip + top)
        {
            if (resultPage.Value != null)
            {
                allSignIns.AddRange(resultPage.Value);
                totalCount += resultPage.Value.Count;
            }

            if (!string.IsNullOrEmpty(resultPage.OdataNextLink))
            {
                var nextPageBuilder = new SignInsRequestBuilder(resultPage.OdataNextLink, graphClient.RequestAdapter);
                resultPage = await nextPageBuilder.GetAsync();
            }
            else
            {
                break;
            }
        }

        return allSignIns
            .Skip(skip)
            .Take(top)
            .Select(signIn => new LoginAttempt
            {
                Id = signIn.Id,
                UserId = signIn.UserId,
                Timestamp = signIn.CreatedDateTime.GetValueOrDefault().DateTime,
                StatusErrorCode = signIn.Status?.ErrorCode,
                OperationalSystem = signIn.DeviceDetail?.OperatingSystem,
                IpAddress = signIn.IpAddress,
                DisplayName = signIn.UserDisplayName,
                City = signIn.Location?.City,
                UserPrincipalName = signIn.UserPrincipalName
            });
    }

    private GraphServiceClient GetGraphClient(string accessToken)
    {
        var tokenProvider = new SimpleAccessTokenProvider(accessToken);
        var authProvider = new BaseBearerTokenAuthenticationProvider(tokenProvider);
        return new GraphServiceClient(authProvider);
    }
}

public class SimpleAccessTokenProvider : IAccessTokenProvider
{
    private readonly string _accessToken;
    public SimpleAccessTokenProvider(string accessToken) => _accessToken = accessToken;

    public Task<string> GetAuthorizationTokenAsync(
        Uri uri,
        Dictionary<string, object> additionalAuthenticationContext = default,
        CancellationToken cancellationToken = default)
        => Task.FromResult(_accessToken);

    public AllowedHostsValidator AllowedHostsValidator { get; } = new AllowedHostsValidator();
}
