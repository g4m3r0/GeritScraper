using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using GeritScraper.DataModels;
using Newtonsoft.Json.Linq;

namespace GeritScraper.Common;

public class ScraperService
{
    
    public static async Task<string?> ScrapeJsonStringFromUrlAsync(string url)
    {
        using var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(url);

        var scriptPattern = @"<script>\s*window\.__PRELOADED_STATE__\s*=(.*?)<\/script>";
        var regex = new Regex(scriptPattern, RegexOptions.Singleline);
        var match = regex.Match(html);

        if (match.Success)
        {
            // Get the whole json which also includes lots of meta data
            var jsonString = match.Groups[1].Value;
            //return jsonString;

            // Validate and format the JSON string
            var jsonObj = JObject.Parse(jsonString);

            // Extract the 'institutionDetail' key and its child elements (containing all information about the institution)
            var institutionDetail = jsonObj["institutionDetail"];
            return institutionDetail.ToString();
        }

        return null;
    }
}