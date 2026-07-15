using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MasterNet.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {

            config.AddInMemoryCollection([
                new KeyValuePair<string, string?>("ConnectionStrings:MasterNetDB", "Server=127.0.0.1,1433;User ID=sa;Password=Vaxi$2025USPass;TrustServerCertificate=true"),
                new KeyValuePair<string, string?>("services:rating-service::http:0", "http://localhost:5069"),
                new KeyValuePair<string, string?>("services:rating-service::https:0", "https://localhost:7191"),
             ]);
        });

        return base.CreateHost(builder);
    }
}
