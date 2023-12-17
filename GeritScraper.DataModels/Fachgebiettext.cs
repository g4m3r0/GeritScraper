using Newtonsoft.Json;

namespace GeritScraper.DataModels;

public class Fachgebiettext
{
    [JsonProperty("de")]
    public string De { get; set; }
    
    [JsonProperty("en")]
    public string En { get; set; }
}