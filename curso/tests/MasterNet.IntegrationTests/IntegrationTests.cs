using System.Net;

namespace MasterNet.IntegrationTests;

public class IntegrationTests
{
    [Fact]
    public async Task TestGetCourses()
    {
        // Arrange
        var waf = new CustomWebApplicationFactory();
        var httpClient = waf.CreateClient();
        // Act
        var response = await httpClient.GetAsync("/api/courses");
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
