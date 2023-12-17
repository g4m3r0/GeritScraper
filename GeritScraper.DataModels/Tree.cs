using Newtonsoft.Json;

namespace GeritScraper.DataModels;

public class Tree
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("name")] public Name Name { get; set; }

    [JsonProperty("children")] public List<ChildrenItem> Children { get; set; }

    public bool IsExpanded { get; set; } = false;
}