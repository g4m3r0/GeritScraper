using Newtonsoft.Json;

namespace GeritScraper.DataModels;

public class Anschrift
{
    [JsonProperty("ort")]
    public string Ort { get; set; }
    
    [JsonProperty("plzvorort")]
    public string Plzvorort { get; set; }
    
    [JsonProperty("strasse")]
    public string Strasse { get; set; }
    
    [JsonProperty("telefon")]
    public Telefon Telefon { get; set; }
    
    [JsonProperty("bundesland")]
    public string? Bundesland { get; set; }
    
    [JsonProperty("postfach")]
    public object Postfach { get; set; }
    
    [JsonProperty("hausnummer")]
    public string Hausnummer { get; set; }
    
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("plznachort")]
    public object Plznachort { get; set; }
    
    [JsonProperty("email")]
    public object Email { get; set; }
}