using MongoDB.Bson.Serialization.Attributes;

namespace GeritScraper.DataModels;

using Newtonsoft.Json;

public class BildTitel
{
    [JsonProperty("de")]
    public string De { get; set; }
    
    [JsonProperty("en")]
    public string En { get; set; }
}

public class EinrichtungsTypText
{
    [JsonProperty("de")]
    public string De { get; set; }
    
    [JsonProperty("en")]
    public string En { get; set; }
}

public class Fullname
{
    [JsonProperty("de")]
    public string De { get; set; }
    
    [JsonProperty("en")]
    public string En { get; set; }
}

public class EinrichtungsArtText
{
    [JsonProperty("de")]
    public string De { get; set; }
    
    [JsonProperty("en")]
    public string En { get; set; }
}

public class PklText
{
    [JsonProperty("de")]
    public string De { get; set; }
    
    [JsonProperty("en")]
    public string En { get; set; }
}

public class Name
{
    [JsonProperty("de")]
    public string De { get; set; }
    
    [JsonProperty("en")]
    public string En { get; set; }
}

public class PfadItem
{
    [JsonProperty("ebene")]
    public object Ebene { get; set; }
    
    [JsonProperty("kurzname")]
    public object Kurzname { get; set; }
    
    [JsonProperty("name")]
    public Name Name { get; set; }
    
    [JsonProperty("id")]
    public int Id { get; set; }
}

public class StabuText
{
    [JsonProperty("de")]
    public string De { get; set; }
    
    [JsonProperty("en")]
    public string En { get; set; }
}

public class Wikipedia
{
    [JsonProperty("de")]
    public string De { get; set; }
    
    [JsonProperty("en")]
    public string En { get; set; }
}

public class Telefon
{
    [JsonProperty("durchwahl")]
    public object Durchwahl { get; set; }
    
    [JsonProperty("laendervorwahl")]
    public object Laendervorwahl { get; set; }
    
    [JsonProperty("ortsvorwahl")]
    public object Ortsvorwahl { get; set; }
}

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
    public string Bundesland { get; set; }
    
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

public class Fachgebiettext
{
    [JsonProperty("de")]
    public string De { get; set; }
    
    [JsonProperty("en")]
    public string En { get; set; }
}

public class Teaser
{
    [JsonProperty("de")]
    public string De { get; set; }
    
    [JsonProperty("en")]
    public string En { get; set; }
}

public class Geolocation
{
    [JsonProperty("lon")]
    public double Lon { get; set; }
    
    [JsonProperty("lat")]
    public double Lat { get; set; }
}

public class ChildrenItem
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public Name Name { get; set; }
    
    public Institution? InstitutionDetails { get; set; }
    
    [JsonProperty("children")]
    public List<ChildrenItem> Children { get; set; }
}

public class Tree
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public Name Name { get; set; }
    
    [JsonProperty("children")]
    public List<ChildrenItem> Children { get; set; }
}

public class Institution
{
    [BsonId]
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("bildtitel")]
    public BildTitel BildTitel { get; set; }
    
    [JsonProperty("job_link")]
    public string JobLink { get; set; }
    
    [JsonProperty("_einrichtungstypText")]
    public EinrichtungsTypText EinrichtungsTypText { get; set; }
    
    [JsonProperty("sumprofessuren")]
    public int? Sumprofessuren { get; set; }
    
    [JsonProperty("_sortNameDe")]
    public string Sortnamede { get; set; }
    
    [JsonProperty("crossreflink_1")]
    public string Crossreflink1 { get; set; }
    
    [JsonProperty("foerderatlas")]
    public string Foerderatlas { get; set; }
    
    [JsonProperty("_realeinrichtungstyp")]
    public int? Realeinrichtungstyp { get; set; }
    
    [JsonProperty("promotion_links_html")]
    public List<string> PromotionLinksHtml { get; set; }
    
    [JsonProperty("logo")]
    public string Logo { get; set; }
    
    [JsonProperty("_wissenschaftsgebiet")]
    public List<string> Wissenschaftsgebiet { get; set; }
    
    [JsonProperty("_einrichtungsart")]
    public string Einrichtungsart { get; set; }
    
    [JsonProperty("fotograf")]
    public string Fotograf { get; set; }
    
    [JsonProperty("_wissenschaftsbereich")]
    public List<string> Wissenschaftsbereich { get; set; }
    
    [JsonProperty("_ortPfad")]
    public string Ortpfad { get; set; }
    
    [JsonProperty("_fullName")]
    public Fullname Fullname { get; set; }
    
    [JsonProperty("_einrichtungsartText")]
    public EinrichtungsArtText EinrichtungsArtText { get; set; }
    
    [JsonProperty("ebene")]
    public object Ebene { get; set; }
    
    [JsonProperty("hauptfachgebiet")]
    public int? Hauptfachgebiet { get; set; }
    
    [JsonProperty("_fachausschuss")]
    public List<string> Fachausschuss { get; set; }
    
    [JsonProperty("_pklText")]
    public PklText PklText { get; set; }
    
    [JsonProperty("pfad")]
    public List<PfadItem> Pfad { get; set; }
    
    [JsonProperty("foto")]
    public string Foto { get; set; }
    
    [JsonProperty("_sortNameEn")]
    public string Sortnameen { get; set; }
    
    [JsonProperty("sumabteilungen")]
    public int? Sumabteilungen { get; set; }
    
    [JsonProperty("name")]
    public Name Name { get; set; }
    
    [JsonProperty("_stabuPfad")]
    public List<string> Stabupfad { get; set; }
    
    [JsonProperty("lizenz_link")]
    public string LizenzLink { get; set; }
    
    [JsonProperty("_fachgebiet")]
    public List<string> Fachgebiet { get; set; }
    
    [JsonProperty("sumstudierende")]
    public int? Sumstudierende { get; set; }
    
    [JsonProperty("_lehrforschungsbereich")]
    public List<string> Lehrforschungsbereich { get; set; }
    
    [JsonProperty("_boostOffset")]
    public int? Boostoffset { get; set; }
    
    [JsonProperty("_insPfad")]
    public string Inspfad { get; set; }
    
    [JsonProperty("rorid")]
    public string Rorid { get; set; }
    
    [JsonProperty("beschreibung")]
    public string? Beschreibung { get; set; }
    
    [JsonProperty("_fächergruppe")]
    public List<object> Fächergruppe { get; set; } // TODO: Update with proper type
    
    [JsonProperty("_isRoot")]
    public bool IsRoot { get; set; }
    
    [JsonProperty("sumfachbereiche")]
    public int? Sumfachbereiche { get; set; }
    
    [JsonProperty("_fach")]
    public List<string>? Fach { get; set; } // TODO: Update with proper type
    
    [JsonProperty("_stabuText")]
    public StabuText StabuText2 { get; set; }
    
    [JsonProperty("bundesland")]
    public int? Bundesland { get; set; }
    
    [JsonProperty("sumbereiche")]
    public int? Sumbereiche { get; set; }
    
    [JsonProperty("sumfachgebiete")]
    public int? Sumfachgebiete { get; set; }
    
    [JsonProperty("fotografprofillink")]
    public string Fotografprofillink { get; set; }
    
    [JsonProperty("wikipedia")]
    public Wikipedia Wikipedia { get; set; }
    
    [JsonProperty("hrk_link")]
    public object HrkLink { get; set; } // TODO: Update with proper type
    
    [JsonProperty("gep_project_count")]
    public int? GepProjectCount { get; set; }
    
    [JsonProperty("pklPfad")]
    public List<object> Pklpfad { get; set; } // TODO: Update with proper type
    
    [JsonProperty("suminstitutionen")]
    public int? Suminstitutionen { get; set; }
    
    [JsonProperty("einrichtungstypundsektion")]
    public string Einrichtungstypundsektion { get; set; }
    
    [JsonProperty("_type")]
    public string Type { get; set; }
    
    [JsonProperty("anschrift")]
    public Anschrift Anschrift { get; set; }
    
    [JsonProperty("url")]
    public string Url { get; set; }
    
    [JsonProperty("crossreflink_3")]
    public object Crossreflink3 { get; set; }
    
    [JsonProperty("ort")]
    public int? Ort { get; set; }
    
    [JsonProperty("crossreflink_2")]
    public object Crossreflink2 { get; set; }
    
    [JsonProperty("_einrichtungstyp")]
    public int? Einrichtungstyp { get; set; }
    
    [JsonProperty("kurzname")]
    public string Kurzname { get; set; }
    
    [JsonProperty("mitglied")]
    public bool Mitglied { get; set; }
    
    [JsonProperty("sumfakultaeten")]
    public int? Sumfakultaeten { get; set; }
    
    [JsonProperty("lizenz")]
    public string Lizenz { get; set; }
    
    [JsonProperty("stabuPfad")]
    public List<string> Stabupfad2 { get; set; } // TODO: Update with proper type
    
    [JsonProperty("einrichtungsart")]
    public object Einrichtungsart2 { get; set; } // TODO: Update with proper type
    
    [JsonProperty("_fachgebietText")]
    public Fachgebiettext Fachgebiettext { get; set; }
    
    [JsonProperty("teaser")]
    public Teaser Teaser { get; set; }
    
    [JsonProperty("promotion")]
    public bool Promotion { get; set; }
    
    [JsonProperty("geolocation")]
    public Geolocation Geolocation { get; set; }
    
    [JsonProperty("tree")]
    public Tree? Tree { get; set; }
}

public class Scrollpositiondetail
{
}

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
