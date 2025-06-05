using AzureBrasil_cloud.Application.Interfaces;
using AzureBrasil_cloud.Domain.Entities;
using Moq;

namespace AzureBrasil_cloud.Tests.Application;

public class UserServiceTests
{
    [Fact]
    public async Task GetCurrentUserAsync_ShouldReturnUser()
    {
        // Arrange
        var token = "fake-token";
        var expectedUser = new User { Id = "123", DisplayName = "John", Email = "john@test.com" };
        var mockGraph = new Mock<IAzureGraphService>();
        mockGraph.Setup(g => g.GetCurrentUserAsync(token)).ReturnsAsync(expectedUser);

        var service = new UserService(mockGraph.Object);

        // Act
        var result = await service.GetCurrentUserAsync(token);

        // Assert
        Assert.Equal(expectedUser.Id, result.Id);
        Assert.Equal(expectedUser.DisplayName, result.DisplayName);
        Assert.Equal(expectedUser.Email, result.Email);
    }

    [Fact]
    public async Task GetUsersAsync_ShouldReturnListOfUsers()
    {
        var token = "fake-token";
        var expectedUsers = new List<User>
        {
            new User { Id = "1", DisplayName = "User1", Email = "u1@test.com" },
            new User { Id = "2", DisplayName = "User2", Email = "u2@test.com" }
        };

        var mockGraph = new Mock<IAzureGraphService>();
        mockGraph.Setup(g => g.GetUsersAsync(token)).ReturnsAsync(expectedUsers);

        var service = new UserService(mockGraph.Object);

        var result = await service.GetUsersAsync(token);

        Assert.Equal(2, result.Count());
        Assert.Contains(result, u => u.Id == "1");
    }
}
