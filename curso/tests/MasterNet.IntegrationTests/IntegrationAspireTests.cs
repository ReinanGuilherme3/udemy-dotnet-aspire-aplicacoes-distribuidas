using Aspire.Hosting.Testing;
using System.Net;

namespace MasterNet.IntegrationTests;

public class IntegrationAspireTests
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);

    [Fact]
    public async Task GetCoursesShouldReturnOk()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.MasterNet_AppHost>();

        await using var app = await appHost.BuildAsync().WaitAsync(DefaultTimeout);
        await app.StartAsync().WaitAsync(DefaultTimeout);

        await app.ResourceNotifications.WaitForResourceHealthyAsync("api").WaitAsync(DefaultTimeout);

        var httpClient = app.CreateHttpClient("api");

        var response = await httpClient.GetAsync("/api/courses");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
