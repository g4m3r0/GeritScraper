using GeritScraper.DataModels;

namespace GeritScraper.DataAdapter;

public interface IMongoInstitutionDataAdapter
{
    Task SaveInstitutionsAsync(IEnumerable<Institution> institutions);
    Task SaveOrUpdateInstitutionAsync(Institution institution);
    Task<List<Institution>> GetFullInstitutionsAsync();
}