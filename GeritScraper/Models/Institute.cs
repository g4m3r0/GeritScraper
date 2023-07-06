namespace GeritScraper.Models;

public class Institute
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string GeritUrl => $"https://www.gerit.org/de/institutiondetail/{this.Id}";

}
