using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MasterNet.AppHost.AppHostEvents;

public static class AppHostEventsExtensions
{
    public static void SubscribeToAppHostEvents(this IDistributedApplicationBuilder builder)
    {
        builder.Eventing.Subscribe<BeforeStartEvent>(static (@event, cancellationToken) =>
        {
            var logger = @event.Services.GetRequiredService<ILogger<BeforeStartEvent>>();
            logger.LogInformation("*****BeforeStartEvent*****");
            return Task.CompletedTask;
        });

        builder.Eventing.Subscribe<ResourceEndpointsAllocatedEvent>(static (@event, cancellationToken) =>
        {
            var logger = @event.Services.GetRequiredService<ILogger<ResourceEndpointsAllocatedEvent>>();
            logger.LogInformation("*****ResourceEndpointsAllocatedEvent***** Resource: {ResourceName}", @event.Resource.Name);
            return Task.CompletedTask;
        });

        builder.Eventing.Subscribe<AfterResourcesCreatedEvent>(static (@event, cancellationToken) =>
        {
            var logger = @event.Services.GetRequiredService<ILogger<AfterResourcesCreatedEvent>>();
            logger.LogInformation("*****AfterResourcesCreatedEvent*****");
            return Task.CompletedTask;
        });
    }
}
