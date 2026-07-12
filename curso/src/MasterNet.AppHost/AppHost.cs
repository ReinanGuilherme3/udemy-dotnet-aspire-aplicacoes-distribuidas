using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("Password", secret: true);

var myParameter = builder.AddParameter("MyParameter");

var myConnectionString = builder.AddConnectionString("MyConnectionString");

var server = builder.AddSqlServer("server", password, 1433).WithLifetime(ContainerLifetime.Persistent);

var db = server.AddDatabase("MasterNetDB");

var api = builder.AddProject<MasterNet_WebApi>("api")
    .WithHttpEndpoint(5001)
    .WithReference(db)
    .WaitFor(db)
    .WithEnvironment("MY_ENV_PARAMETER", myParameter)
    .WithReference(myConnectionString);

var client = builder.AddProject<MasterNet_Client>("client")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.AddProject<MasterNet_MigrationService>("migration")
    .WithReference(db)
    .WaitFor(db)
    .WithParentRelationship(server);

builder.AddProject<Projects.MasterNet_RatingService>("masternet-ratingservice");

builder.Build().Run();
