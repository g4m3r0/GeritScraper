using Newtonsoft.Json;

namespace GeritScraper.DataModels;

public class RootObject
{
    [JsonProperty("openedInstitutions")]
    public List<object> Openedinstitutions { get; set; } // TODO: Update with proper type
    
    [JsonProperty("allOpen")]
    public bool Allopen { get; set; }
    
    [JsonProperty("jsonOn")]
    public bool Jsonon { get; set; }
    
    [JsonProperty("activeMenu")]
    public object Activemenu { get; set; }
    
    [JsonProperty("institution")]
    public Institution Institution { get; set; }
    
    [JsonProperty("rootInstitutionId")]
    public object Rootinstitutionid { get; set; }
    
    [JsonProperty("teaserExpanded")]
    public bool TeaserExpanded { get; set; }
    
    [JsonProperty("associatedInstitution")]
    public object Associatedinstitution { get; set; }
    
    [JsonProperty("historyAction")]
    public object Historyaction { get; set; }
    
    [JsonProperty("scrollPositionDetail")]
    public Scrollpositiondetail Scrollpositiondetail { get; set; }
    
    [JsonProperty("pfadInstitutionen")]
    public List<object> Pfadinstitutionen { get; set; } // TODO: Update with proper type
    
    [JsonProperty("loading")]
    public bool Loading { get; set; }
    
    [JsonProperty("error")]
    public object Error { get; set; }
}