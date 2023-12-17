using System.Diagnostics;
using System.Text;
using GeritScraper.Common;
using GeritScraper.DataAdapter;
using GeritScraper.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GeritScraper.JsonExtractor.Console.Test;

public class JsonExtractor
{
    private static MongoInstitutionDataAdapter mongoDbAdapter  = new MongoInstitutionDataAdapter(
        "mongodb+srv://AdminLu:&%406TmNYccF4k24iJ6kCh@academicjobs.lgwya1x.mongodb.net/?retryWrites=true&w=majority",
        "Institutions", "institutionCollection");
    
    private static readonly int _delayInMs = 0;
    private static readonly string _productVersion = "1.0";
    private static readonly string _contactInformation = "Contact: lucas.schmutzler@s2018.tu-chemnitz.de";

    private static ScraperService _scraperService;
    public async Task RunExtractor(string inputPath)
    {
        _scraperService = new ScraperService(_contactInformation, _productVersion, _delayInMs);
        
        // Get all Json Files (one file represents one institution)
        string[] filePaths = Directory.GetFiles(inputPath, "*.json", SearchOption.AllDirectories);

        var allInstitutes = new List<Institution>();

        foreach (var path in filePaths)
        {
            try
            {
                var institute = DeserializeSingleInstituteFromFile(path);
                var url = $"https://www.gerit.org/de/institutiondetail/{institute.Id}";
                Debug.WriteLine(
                    $"Found {institute.Name.De}, ID: {institute.Id}, Url: {url}"); // TODO log to file

                allInstitutes.Add(institute);

                await LoopTreeChildren(institute.Tree.Children);

                await mongoDbAdapter.SaveOrUpdateInstitutionAsync(institute);
            }
            catch (Exception e)
            {
                throw;
                // Ignore for now
            }
        }

        var allInstitutesInDb = await mongoDbAdapter.GetFullInstitutionsAsync();
        Debug.WriteLine($"{allInstitutesInDb.Count} Institutions in DB.");
        Debug.WriteLine("Ended");
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
        Debug.WriteLine($"    Found Child {child.Name.De}, ID: {child.Id}, Url: {url}"); // TODO log to file

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
            Debug.WriteLine(e.Message);
        }
    }

    private static Institution DeserializeSingleInstituteFromFile(string jsonFilePath)
    {
        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException();
        }

        // Read the content of the file
        string jsonContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

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
            },
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
            Debug.WriteLine($"An error happened while deserialization: {e}");
            throw;
        }
    }
}