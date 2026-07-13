namespace MasterNet.Application.Contracts;

public interface IRatingServiceHttpClient
{
    Task<int> GetRating(string id);
    Task SendRating(string id, int rating);
}
