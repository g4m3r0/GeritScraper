using MongoDB.Driver;
using ProfJobs.NLP.Common;

namespace GeritScraper.DataAdapter;

public class MongoVacancyDataAdapter : IMongoVacancyDataAdapter
{
    private readonly IMongoCollection<Vacancy> _vacancyCollection;

    public MongoVacancyDataAdapter(string connectionString, string databaseName, string collectionName)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);

        // Create a new client and connect to the server
        var client = new MongoClient(settings);

        var database = client.GetDatabase(databaseName);
        _vacancyCollection = database.GetCollection<Vacancy>(collectionName);
    }

    public async Task SaveOrUpdateVacancyAsync(Vacancy vac)
    {
        var filter = Builders<Vacancy>.Filter.Eq(j => j.Id, vac.Id);
        await _vacancyCollection.ReplaceOneAsync(filter, vac, new ReplaceOptions { IsUpsert = true });
    }

    public async Task<List<Vacancy>> GetVacanciesAsync()
    {
        return await _vacancyCollection.Find(i => true).ToListAsync();
    }
}