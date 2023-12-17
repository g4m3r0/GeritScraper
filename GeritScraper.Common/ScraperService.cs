using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace GeritScraper.Common;

public class ScraperService
{
    private readonly int _delayInMs = 1000;
    private readonly HttpClient _httpClient = new();

    public ScraperService(string contactInfo, string productVersion = "1.0", int delayInMs = 1000)
    {
        _delayInMs = delayInMs;

        _httpClient.DefaultRequestHeaders.UserAgent.Clear();
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ProfJobs",
            productVersion));
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue($"({contactInfo})"));
    }

    public async Task<string?> ScrapeJsonStringFromUrlAsync(string url)
    {
        var html = await _httpClient.GetStringAsync(url);

        var scriptPattern = @"<script>\s*window\.__PRELOADED_STATE__\s*=(.*?)<\/script>";
        var regex = new Regex(scriptPattern, RegexOptions.Singleline);
        var match = regex.Match(html);

        if (match.Success)
        {
            // Get the whole json which also includes lots of meta data
            var jsonString = match.Groups[1].Value;

            // Validate and format the JSON string
            var jsonObj = JObject.Parse(jsonString);

            // Extract the 'institutionDetail' key and its child elements (containing all information about the institution)
            var institutionDetail = jsonObj["institutionDetail"];

            // Delay between next scrape
            await Task.Delay(_delayInMs);

            return institutionDetail.ToString();
        }

        return null;
    }
}