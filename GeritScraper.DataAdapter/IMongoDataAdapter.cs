using GeritScraper.DataModels;

namespace GeritScraper.DataAdapter;

public interface IMongoDataAdapter
{
    Task SaveInstitutionsAsync(IEnumerable<Institution> institutions);
    Task SaveOrUpdateInstitutionAsync(Institution institution);
    Task<List<Institution>> GetFullJobsAsync();
}