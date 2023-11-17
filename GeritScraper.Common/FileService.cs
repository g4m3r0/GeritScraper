namespace GeritScraper.Common;

public class FileService
{
    public string GetDatabasePath() => Path.Combine(Directory.GetCurrentDirectory(), "Input", "institutionen_gerit.csv");
    public string GetOutputPath() => Path.Combine(Directory.GetCurrentDirectory(), "Output_new");
}