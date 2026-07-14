using MasterNet.RatingService;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddCors();

builder.AddRedisClient(connectionName: "cache");

builder.Services.AddSingleton<RatingStore>();

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapPost("/ratings", (RatingStore store, RatingRequest request) =>
{
    Activity.Current?.AddEvent(new ActivityEvent("Ocurrio un evento inesperado"));

    if (string.IsNullOrWhiteSpace(request.Id))
        return Results.BadRequest("Id is required");

    store.AddRating(request.Id, request.Rating);

    Activity.Current?.SetTag("CourseId", request.Id);
    Activity.Current?.SetTag("Rating", request.Rating);

    return Results.Ok();
});

app.MapGet("/ratings/{id}", async (RatingStore store, string id) =>
{
    if (string.IsNullOrWhiteSpace(id))
        return Results.BadRequest("Id is required");

    var rating = await store.GetAverangeRating(id);
    if (rating == null)
        return Results.NotFound();

    Activity.Current?.SetTag("CourseId", id);
    Activity.Current?.SetTag("Rating", rating);

    return Results.Ok(rating);
});

app.MapDefaultEndpoints();

app.Run();

public record RatingRequest(string Id, int Rating);