using Newtonsoft.Json;

namespace GeritScraper.DataModels;

public class PfadItem
{
    [JsonProperty("ebene")] public object Ebene { get; set; }

    [JsonProperty("kurzname")] public object Kurzname { get; set; }

    [JsonProperty("name")] public Name Name { get; set; }

    [JsonProperty("id")] public int Id { get; set; }
}