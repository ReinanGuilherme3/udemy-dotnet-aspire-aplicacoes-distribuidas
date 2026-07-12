using StackExchange.Redis;

namespace MasterNet.RatingService;

public class RatingStore(IConnectionMultiplexer connection)
{
    public void AddRating(string id, int rating)
    {
        var db = connection.GetDatabase();
        db.ListRightPushAsync(id, rating);
    }

    public async Task<int> GetAverangeRating(string id)
    {
        var db = connection.GetDatabase();
        var ratings = await db.ListRangeAsync(id);

        if (ratings.Length == 0)
        {
            return 0;
        }

        var promedio = Math.Round(ratings.Select(r => (int)r).Average(), 0);

        return (int)promedio;
    }
}
