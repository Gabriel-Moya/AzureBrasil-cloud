using AzureBrasil_cloud.Application.Interfaces;
using AzureBrasil_cloud.Application.Services;
using AzureBrasil_cloud.Domain.Entities;
using Moq;

public class TenantServiceTests
{
    [Fact]
    public async Task GetTenantAsync_ReturnsTenant()
    {
        // Arrange
        var token = "fake-token";
        var expectedTenant = new Tenant { Id = "tenant-1", DisplayName = "Tenant Corp" };
        var mockGraph = new Mock<IAzureGraphService>();
        mockGraph.Setup(g => g.GetTenantAsync(token)).ReturnsAsync(expectedTenant);

        var service = new TenantService(mockGraph.Object);

        // Act
        var result = await service.GetTenantAsync(token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTenant.Id, result.Id);
        Assert.Equal(expectedTenant.DisplayName, result.DisplayName);
    }
}
