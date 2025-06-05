using AzureBrasil_cloud.Application.Interfaces;
using AzureBrasil_cloud.Application.Services;
using AzureBrasil_cloud.Domain.Entities;
using Moq;

public class GroupServiceTests
{
    [Fact]
    public async Task GetGroupsAsync_ReturnsGroupsList()
    {
        // Arrange
        var token = "fake-token";
        var expectedGroups = new List<Group>
        {
            new Group { Id = "1", DisplayName = "Admin", Description = "Admin group" },
            new Group { Id = "2", DisplayName = "Users", Description = "Users group" }
        };

        var mockGraph = new Mock<IAzureGraphService>();
        mockGraph.Setup(g => g.GetGroupsAsync(token)).ReturnsAsync(expectedGroups);

        var service = new GroupService(mockGraph.Object);

        // Act
        var result = await service.GetGroupsAsync(token);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, g => g.DisplayName == "Admin");
    }
}
