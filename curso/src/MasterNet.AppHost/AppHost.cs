using MasterNet.AppHost.AppHostEvents;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("Password", secret: true);

var myParameter = builder.AddParameter("MyParameter");

var myConnectionString = builder.AddConnectionString("MyConnectionString");

var server = builder.AddSqlServer("server", password, 1433)
    .WithLifetime(ContainerLifetime.Persistent);

var db = server.AddDatabase("MasterNetDB");

var cache = builder.AddRedis("cache")
    .WithRedisCommander()
    .WithLifetime(ContainerLifetime.Persistent);

var ratingService = builder.AddProject<MasterNet_RatingService>("rating-service")
    .WithReference(cache)
    .WaitFor(cache);

var api = builder.AddProject<MasterNet_WebApi>("api")
    .WithHttpEndpoint(5001)
    .WithReference(db)
    .WithReference(ratingService)
    .WaitFor(db)
    .WaitFor(ratingService);

var client = builder.AddProject<MasterNet_Client>("client")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.AddProject<MasterNet_MigrationService>("migration")
    .WithReference(db)
    .WaitFor(db)
    .WithParentRelationship(server);

//builder.SubscribeToAppHostEvents();
builder.SubscribeToResourceEvents(api.Resource);

builder.Build().Run();
