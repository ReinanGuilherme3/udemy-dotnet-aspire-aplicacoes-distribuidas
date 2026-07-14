using MasterNet.Application.Contracts;

namespace MasterNet.WebApi;

public class RatingServiceHttpClient(HttpClient httpClient, ILogger<RatingServiceHttpClient> logger) : IRatingServiceHttpClient
{
    public async Task<int> GetRating(string id)
    {
        var response = await httpClient.GetAsync($"/ratings/{id}");

        if (!response.IsSuccessStatusCode)
            return 0;

        return await response.Content.ReadFromJsonAsync<int>();
    }

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
