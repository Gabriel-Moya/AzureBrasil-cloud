namespace AzureBrasil_cloud.Domain.Entities;

public class LoginAttempt
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public int? StatusErrorCode { get; set; }
    public string OperationalSystem { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string UserPrincipalName { get; set; } = string.Empty;
}
