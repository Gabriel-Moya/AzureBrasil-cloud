using AzureBrasil_cloud.Application.Interfaces;
using AzureBrasil_cloud.Application.Services;
using AzureBrasil_cloud.Domain.Entities;
using Moq;

namespace AzureBrasil_cloud.Tests.Application;

public class LoginAttemptServiceTests
{
    [Fact]
    public async Task GetLoginAttemptAsync_ShouldReturnPaginatedList()
    {
        // Arrange
        var token = "fake-token";
        var skip = 0;
        var top = 2;
        var expected = new List<LoginAttempt>
        {
            new LoginAttempt { Id = "a1", UserId = "u1" },
            new LoginAttempt { Id = "a2", UserId = "u2" }
        };

        var mockGraph = new Mock<IAzureGraphService>();
        mockGraph.Setup(g => g.GetLoginAttemptsAsync(token, skip, top)).ReturnsAsync(expected);

        var service = new LoginAttemptService(mockGraph.Object);

        // Act
        var result = await service.GetLoginAttemptsAsync(token, skip, top);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("u1", result.First().UserId);
    }
}
