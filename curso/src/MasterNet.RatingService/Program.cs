using MasterNet.RatingService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddCors();

builder.AddRedisClient(connectionName: "cache");

builder.Services.AddSingleton<RatingStore>();

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapPost("/ratings", (RatingStore store, RatingRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Id))
        return Results.BadRequest("Id is required");

    store.AddRating(request.Id, request.Rating);
    return Results.Ok();
});

app.MapGet("/ratings/{id}", (RatingStore store, string id) =>
{
    if (string.IsNullOrWhiteSpace(id))
        return Results.BadRequest("Id is required");

    var rating = store.GetAverangeRating(id);
    if (rating == null)
        return Results.NotFound();

    return Results.Ok(rating);
});

app.MapDefaultEndpoints();

app.Run();

public record RatingRequest(string Id, int Rating);