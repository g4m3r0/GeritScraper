using GeritScraper.DataModels;
using MongoDB.Driver;

namespace GeritScraper.DataAdapter;

public class MongoDataAdapter : IMongoDataAdapter
{
    private readonly IMongoCollection<Institution> _institutionCollection;

    public MongoDataAdapter(string connectionString, string databaseName, string collectionName)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);
            
        // Create a new client and connect to the server
        var client = new MongoClient(settings);
        
        var database = client.GetDatabase(databaseName);
        _institutionCollection = database.GetCollection<Institution>(collectionName);
    }

    public async Task SaveInstitutionsAsync(IEnumerable<Institution> institutions)
    {
        foreach (var institution in institutions)
        {
            this.SaveOrUpdateInstitutionAsync(institution);
        }
    }

    public async Task SaveOrUpdateInstitutionAsync(Institution institution)
    {
        var filter = Builders<Institution>.Filter.Eq(j => j.Id, institution.Id);
        await _institutionCollection.ReplaceOneAsync(filter, institution, new ReplaceOptions { IsUpsert = true });
    }
    
    public async Task<List<Institution>> GetFullJobsAsync()
    {
        return await _institutionCollection.Find(i => true).ToListAsync();
    }
}