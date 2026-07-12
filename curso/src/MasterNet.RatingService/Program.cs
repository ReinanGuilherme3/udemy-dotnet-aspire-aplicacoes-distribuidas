using MasterNet.RatingService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddCors();

builder.AddRedisClient(connectionName: "cache");

builder.Services.AddSingleton<RatingStore>();

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapDefaultEndpoints();

app.Run();
