using Newtonsoft.Json;

namespace GeritScraper.DataModels;

public class Fullname
{
    [JsonProperty("de")]
    public string De { get; set; }
    
    [JsonProperty("en")]
    public string En { get; set; }
}