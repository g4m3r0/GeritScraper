using GeritScraper.DataModels;
using Newtonsoft.Json.Linq;

namespace GeritScraper.Common;

public class Utilities
{
    public static string GetInstitutionNameFromJson(string jsonString)
    {
        var jsonObj = JObject.Parse(jsonString);
        var institution = jsonObj["institution"];
        var fullName = institution["_fullName"]["de"]; // we could also use the english name using ["en"]

        return fullName.ToString();
    }
    
    public static string ToValidFileName(string fileName) =>
        Path.GetInvalidFileNameChars().Aggregate(fileName, (f, c) => f.Replace(c, '_'));
}