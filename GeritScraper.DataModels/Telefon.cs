using Newtonsoft.Json;

namespace GeritScraper.DataModels;

public class Telefon
{
    [JsonProperty("durchwahl")] public object Durchwahl { get; set; }

    [JsonProperty("laendervorwahl")] public object Laendervorwahl { get; set; }

    [JsonProperty("ortsvorwahl")] public object Ortsvorwahl { get; set; }
}