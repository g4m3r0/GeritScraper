using System.Diagnostics;
using System.Text;
using GeritScraper.Common;
using GeritScraper.DataAdapter;
using GeritScraper.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GeritScraper.JsonExtractor;

public class JsonExtractor
{
    private MongoInstitutionDataAdapter mongoDbAdapter;

    private static readonly int _delayInMs = 0;
    private static readonly string _productVersion = "1.0";
    private static readonly string _contactInformation = "Contact: lucas.schmutzler@s2018.tu-chemnitz.de";

    private static ScraperService? _scraperService;

    public async Task RunExtractor(string inputPath)
    {
        try
        {
            var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING") ??
                                        "mongodb://admin:superSecurePassword!@mongo:27017";
            mongoDbAdapter = new(
                //"mongodb+srv://AdminLu:&%406TmNYccF4k24iJ6kCh@academicjobs.lgwya1x.mongodb.net/?retryWrites=true&w=majority",
                mongoConnectionString,
                "Institutions", "institutionCollection");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        try
        {
            var allInstitutesInDb1 = await mongoDbAdapter.GetFullInstitutionsAsync();
            Console.WriteLine($"{allInstitutesInDb1.Count} Institutions in DB.");

            _scraperService = new ScraperService(_contactInformation, _productVersion, _delayInMs);

            // Get all Json Files (one file represents one institution)
            Console.WriteLine($"Get all Json Files in {inputPath}");
            var filePaths = Directory.GetFiles(inputPath, "*.json", SearchOption.AllDirectories);

            var allInstitutes = new List<Institution>();

            foreach (var path in filePaths)
            {
                var institute = DeserializeSingleInstituteFromFile(path);
                var url = $"https://www.gerit.org/de/institutiondetail/{institute.Id}";
                Console.WriteLine(
                    $"Found {institute.Name.De}, ID: {institute.Id}, Url: {url}");

                allInstitutes.Add(institute);

                await LoopTreeChildren(institute.Tree.Children);

                await mongoDbAdapter.SaveOrUpdateInstitutionAsync(institute);
            }

            var allInstitutesInDb = await mongoDbAdapter.GetFullInstitutionsAsync();
            Console.WriteLine($"{allInstitutesInDb.Count} Institutions in DB.");
            Console.WriteLine("Ended");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static async Task LoopTreeChildren(List<ChildrenItem> children)
    {
        if (children == null || !children.Any())
            return;

        foreach (var child in children)
        {
            // Process the child item
            await ProcessChild(child);

            // Recursively loop through the child's children
            LoopTreeChildren(child.Children);
        }
    }

    private static async Task ProcessChild(ChildrenItem child)
    {
        var url = $"https://www.gerit.org/de/institutiondetail/{child.Id}";
        Console.WriteLine($"    Found Child {child.Name.De}, ID: {child.Id}, Url: {url}"); // TODO log to file

        try
        {
            // Get Json and parse it into a C# object
            var jsonString = await _scraperService.ScrapeJsonStringFromUrlAsync(url);
            var institutionDetails = DeserializeSingleInstitute(jsonString);

            // Clear Tree to reduce duplicate data
            institutionDetails.Tree = null;

            child.InstitutionDetails = institutionDetails;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static Institution DeserializeSingleInstituteFromFile(string jsonFilePath)
    {
        if (!File.Exists(jsonFilePath)) throw new FileNotFoundException();

        // Read the content of the file
        var jsonContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

        return DeserializeSingleInstitute(jsonContent);
    }

    private static Institution DeserializeSingleInstitute(string jsonContent)
    {
        // JSON Serialization Settings
        var jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = true,
                    OverrideSpecifiedNames = false
                }
            }
            // Add other settings if needed, such as handling nulls or default values
        };

        // Deserialize the JSON content to the RootObject class
        // Replace 'RootObject' with the actual name of your root class
        try
        {
            var deserializedData = JsonConvert.DeserializeObject<RootObject>(jsonContent, jsonSettings);
            return deserializedData.Institution;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error happened while deserialization: {e}");
            throw;
        }
    }
}