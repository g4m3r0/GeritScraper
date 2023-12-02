using ProfJobs.NLP.Common;

namespace GeritScraper.DataAdapter;

public interface IMongoVacancyDataAdapter
{
    Task SaveOrUpdateVacancyAsync(Vacancy vac);
    Task<List<Vacancy>> GetVacanciesAsync();
}