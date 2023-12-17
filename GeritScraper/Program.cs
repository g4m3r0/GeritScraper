using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using CsvHelper;
using GeritScraper.Common;
using GeritScraper.DataModels;
using GeritScraper.JsonExtractor.Console.Test;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Program
{
    private const string DfgGeritExcelLink = "https://gerit.org/downloads/institutionen_gerit.xlsx";

    private static WebClient _wClient = new WebClient();

    private static string _databasePath = string.Empty;
    private static string _outputPath = string.Empty;

    private static int _delayInMs = 0;
    private static string _productVersion = "1.0";
    private static string _contactInformation = "Contact: lucas.schmutzler@s2018.tu-chemnitz.de";

    private static ScraperService _scraperService;

    static async Task Main(string[] args)
    {
        // Setup initial value
        _scraperService = new ScraperService(_contactInformation, _productVersion, _delayInMs);
        _databasePath = Directory.GetCurrentDirectory() + "\\Input\\institutionen_gerit.xlsx";
        _outputPath = Directory.GetCurrentDirectory() + "\\Output\\";

        // Run the Gerit Scraper to scrape all parent institutes to a json file
        await ScrapeGerit();
        await Console.Out.WriteLineAsync("Finished ScrapeGerit");

        // Run the json extractor to scrape all child institutes and add them to the database.
        await Console.Out.WriteLineAsync("Starting JsonExtractor");
        var extractor = new JsonExtractor();
        await extractor.RunExtractor(_outputPath);
        await Console.Out.WriteLineAsync("Finished JsonExtractor");
    }

    static async Task ScrapeGerit()
    {
        // Scrapes the DFG GERiT institutes database file for the IDs of the institutions
        // Then scraping the DFG GERiT web catalog according to these IDs to extract the JSON object representation of these institutions
        await Console.Out.WriteLineAsync("Starting ScrapeGerit");
        await Console.Out.WriteLineAsync($"Download DFG GERiT Excel Database to {_databasePath}.");

        try
        {
            await _wClient.DownloadFileTaskAsync(new Uri(DfgGeritExcelLink), _databasePath);
            await Console.Out.WriteLineAsync("Finished downloading the Excel Database.");

            var institutionUrls = await ParseGeritDatabaseForUrls(_databasePath);
            await ScrapeUrlsAsync(institutionUrls, _outputPath);

            await Console.Out.WriteLineAsync($"Finished scraping all URLs!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    static async Task<List<string>> ParseGeritDatabaseForUrls(string databasePath)
    {
        await Console.Out.WriteLineAsync("Loading URLs from CSV datasource.");
        var urls = LoadUrlsFromExcel(databasePath);

        await Console.Out.WriteLineAsync($"Loaded {urls.Count} URLs.");

        return urls;
    }

    static async Task ScrapeUrlsAsync(List<string> Urls, string outputPath)
    {
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        StringBuilder scrapeLog = new();

        // Loop each institute
        foreach (var url in Urls)
        {
            // Get the json string from the main institute
            var jsonString = string.Empty;
            var lastErrorMessage = string.Empty;

            try
            {
                jsonString = await _scraperService.ScrapeJsonStringFromUrlAsync(url);
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
            }

            if (!string.IsNullOrEmpty(jsonString))
            {
                var universityName = Utilities.ToValidFileName(Utilities.GetInstitutionNameFromJson(jsonString));
                await Console.Out.WriteLineAsync($"Success - Scraped {universityName} on {url}.");

                if (!Directory.Exists(outputPath + universityName))
                {
                    Directory.CreateDirectory(outputPath + universityName);
                }

                var outputFileName = outputPath + universityName + "\\" + url.Split('/').Last() + ".json";
                await File.WriteAllTextAsync(outputFileName, jsonString);
            }
            else
            {
                await Console.Out.WriteLineAsync($"Failed - Scraped {url}.");
            }

            var status = lastErrorMessage == string.Empty ? "Success" : "Failed";
            scrapeLog.Append($"{status},{url},{DateTime.Now.ToString()},{lastErrorMessage}{Environment.NewLine}");
        }

        await File.WriteAllTextAsync(outputPath + "\\scrapeLog.csv", scrapeLog.ToString());
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

    private static List<string> LoadUrlsFromExcel(string excelFilePath)
    {
        var urls = new List<string>();

        // Open the Excel file
        using (var workbook = new XLWorkbook(excelFilePath))
        {
            // Assume the data is in the first worksheet and the URLs are in a specific column, e.g., 'A'
            var worksheet = workbook.Worksheet(1);

            // Iterate through the rows
            foreach (var row in worksheet.RangeUsed().Rows())
            {
                // Assume URLs are in the first column
                var geritId = row.Cell(1).GetValue<string>();
                if (!string.IsNullOrEmpty(geritId))
                {
                    urls.Add($"https://www.gerit.org/en/institutiondetail/{geritId}");
                }
            }
        }

        return urls;
    }

    private static async Task ParseFilesForInstitutes()
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

    private static async Task<List<Institute>> ScrapeInstitutesForUrl(List<Institute> institutes)
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

    private static List<Institute> GetAllInstitutesFromJson(string jsonString)
    {
        var jsonObj = JObject.Parse(jsonString);
        var institutes = new List<Institute>();

        var institutionRoot = jsonObj["institution"]["tree"];

        // Recursively parse all children
        ExtractIds(institutionRoot, institutes);

        return institutes;
    }

    private static void ExtractIds(JToken token, List<Institute> institutes)
    {
        if (token["id"] != null)
        {
            var id = (int)token["id"];
            var name = token["name"]["de"].ToString();

            institutes.Add(new Institute() { Id = id, Name = name });
        }

        // Recursively parse all children
        if (token["children"] != null)
        {
            foreach (var child in token["children"])
            {
                ExtractIds(child, institutes);
            }
        }
    }
}