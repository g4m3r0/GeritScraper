using Newtonsoft.Json;

namespace GeritScraper.DataModels;

public class ChildrenItem
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("name")] public Name Name { get; set; }

    public Institution? InstitutionDetails { get; set; }

    [JsonProperty("children")] public List<ChildrenItem> Children { get; set; }
    
    public bool IsChildExpanded { get; set; } = false;
}