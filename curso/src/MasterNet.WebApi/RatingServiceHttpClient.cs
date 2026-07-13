using MasterNet.Application.Contracts;

namespace MasterNet.WebApi;

public class RatingServiceHttpClient(HttpClient httpClient, ILogger<RatingServiceHttpClient> logger) : IRatingServiceHttpClient
{
    public Task<int> GetRating(string id) =>
        httpClient.GetFromJsonAsync<int>($"/ratings/{id}");

    public Task SendRating(string id, int rating)
    {
        logger.LogInformation("Sending rating {Rating} for id {Id}", rating, id);

        var request = new
        {
            Id = id,
            Rating = rating
        };

        return httpClient.PostAsJsonAsync("/ratings", request);
    }
}
