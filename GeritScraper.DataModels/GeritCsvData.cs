using CsvHelper.Configuration.Attributes;

namespace GeritScraper.DataModels;

public class GeritCsvData
{
    [Name("DFG Instituts ID")] public string DfgInstitutsId { get; set; }

    [Name("ROR ID")] public string RorId { get; set; }

    [Name("Name deutsch")] public string NameDeutsch { get; set; }

    [Name("Name englisch")] public string NameEnglisch { get; set; }

    [Name("Strasse")] public string Strasse { get; set; }

    [Name("Hausnummer")] public string Hausnummer { get; set; }

    [Name("Postfach")] public string Postfach { get; set; }

    [Name("Postleitzahl vor Ort")] public string PostleitzahlVorOrt { get; set; }

    [Name("Ort")] public string Ort { get; set; }

    [Name("Postleitzahl nach Ort")] public string PostleitzahlNachOrt { get; set; }

    [Name("Einrichtungsart")] public string Einrichtungsart { get; set; }

    [Name("Einrichtungsart englisch")] public string EinrichtungsartEnglisch { get; set; }

    [Name("DESTATIS Fächergruppe")] public string DestatisFachergruppe { get; set; }

    [Name("DESTATIS Fächergruppe englisch")]
    public string DestatisFachergruppeEnglisch { get; set; }

    [Name("DESTATIS Lehr- Forschungsbereich")]
    public string DestatisLehrForschungsbereich { get; set; }

    [Name("DESTATIS Lehr- Forschungsbereich englisch")]
    public string DestatisLehrForschungsbereichEnglisch { get; set; }

    [Name("DESTATIS Fachgebiet")] public string DestatisFachgebiet { get; set; }

    [Name("DESTATIS Fachgebiet englisch")] public string DestatisFachgebietEnglisch { get; set; }

    [Name("URL der Einrichtung")] public string UrlDerEinrichtung { get; set; }

    [Name("URL GERiT Nachweis")] public string UrlGeritNachweis { get; set; }
}