using Newtonsoft.Json;

namespace GeritScraper.DataModels;

public class Geolocation
{
    [JsonProperty("lon")]
    public double Lon { get; set; }
    
    [JsonProperty("lat")]
    public double Lat { get; set; }
}