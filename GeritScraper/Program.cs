using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using CsvHelper;
using GeritScraper.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Program
{
    static async Task Main(string[] args)
    {
        await ParseFilesForInstitutes();

        await Console.Out.WriteLineAsync("Starting the Scraper");
        await Console.Out.WriteLineAsync("Loading URLs from CSV datasource.");

        var dataPath = Directory.GetCurrentDirectory() + "\\Input\\institutionen_gerit.csv";
        var urls = LoadUrlsFromCsv(dataPath);

        var outputPath = Directory.GetCurrentDirectory() + "\\Output\\";

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        await Console.Out.WriteLineAsync($"Loaded {urls.Count} URLs.");

        foreach (var url in urls)
        {
            var jsonString = await ScrapeJsonStringFromUrlAsync(url);

            if (!string.IsNullOrEmpty(jsonString))
            {
                var universityName = ToValidFileName(GetInstitutionNameFromJson(jsonString));
                await Console.Out.WriteLineAsync($"Success - Scraped {universityName} on {url}.");

                if (!Directory.Exists(outputPath + universityName))
                {
                    Directory.CreateDirectory(outputPath + universityName);
                }

                var outputFileName = outputPath + universityName + "\\" + url.Split('/').Last() + ".json";
                await File.WriteAllTextAsync(outputFileName, jsonString);

                // await Console.Out.WriteLineAsync($"Press Enter to scrape next");
                // Console.ReadLine();
            } else
            {
                await Console.Out.WriteLineAsync($"Failed - Scraped {url}.");
            }
        }

        await Console.Out.WriteLineAsync($"Finished scraping all URLs!");
        Console.ReadLine();
    }

    static async Task<string?> ScrapeJsonStringFromUrlAsync(string url)
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

    static string GetInstitutionNameFromJson(string jsonString)
    {
        // TODO handle excetions / null reference errors

        var jsonObj = JObject.Parse(jsonString);
        var institution = jsonObj["institution"];
        var fullName = institution["_fullName"]["de"]; // we could also use the english name using ["en"]

        return fullName.ToString();
    }

    static List<string> LoadUrlsFromCsv(string csvFilePath)
    {
        using var reader = new StreamReader(csvFilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = new List<GeritCsvData>();
        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            var record = csv.GetRecord<GeritCsvData>();
            records.Add(record);
        }

        var urls = new List<string>();
        foreach (var record in records)
        {
            urls.Add(record.UrlGeritNachweis);
        }

        return urls;
    }

    static async Task ParseFilesForInstitutes()
    {
        var outputPath = Directory.GetCurrentDirectory() + "\\Output\\";

        foreach (var dir in Directory.GetDirectories(outputPath))
        {
            foreach (var file in Directory.GetFiles(dir))
            {
                if (file.Contains("institutes.json"))
                {
                    continue;
                }

                var jsonString = File.ReadAllText(file);
                var institutes = GetAllInstitutesFromJson(jsonString);

                var institutesWithUrls = await ScrapeInstitutesForUrl(institutes);

                // Serialize the model to JSON
                var institutesJson = JsonConvert.SerializeObject(institutesWithUrls, Formatting.Indented);

                // Save the JSON to a file
                File.WriteAllText(dir + "\\institutes.json", institutesJson);
            }
        }
    }

    static async Task<List<Institute>> ScrapeInstitutesForUrl(List<Institute> institutes)
    {
        using var httpClient = new HttpClient();

        foreach (Institute instute in institutes)
        {
            string htmlString;

            try
            {
                htmlString = await httpClient.GetStringAsync(instute.GeritUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{instute.GeritUrl}: {ex.Message}");
                continue;
            }

            var linkPattern = @"<span id=""institutionHomepage"">.*?<a href=""(.*?)"".*?>";
            var regex = new Regex(linkPattern, RegexOptions.Singleline);
            var match = regex.Match(htmlString);

            if (match.Success)
            {
                var link = match.Groups[1].Value;
                Console.WriteLine($"Extracted link: {link}");

                instute.Url = link;
            }
            else
            {
                Console.WriteLine("No link found.");
            }
        }

        return institutes;
    }

    static List<Institute> GetAllInstitutesFromJson(string jsonString)
    {
        var jsonObj = JObject.Parse(jsonString);
        var institutes = new List<Institute>();

        var institutionRoot = jsonObj["institution"]["tree"];

        // Recursivly parse all children
        ExtractIds(institutionRoot, institutes);

        return institutes;
    }

    static void ExtractIds(JToken token, List<Institute> institutes)
    {
        if (token["id"] != null)
        {
            var id = (int)token["id"];
            var name = token["name"]["de"].ToString();

            institutes.Add(new Institute() { Id = id, Name = name });
        }

        // Recursivly parse all children
        if (token["children"] != null)
        {
            foreach (var child in token["children"])
            {
                ExtractIds(child, institutes);
            }
        }
    }

    public static string ToValidFileName(string fileName) => Path.GetInvalidFileNameChars().Aggregate(fileName, (f, c) => f.Replace(c, '_'));
}
