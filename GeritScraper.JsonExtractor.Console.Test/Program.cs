using System.Text;
using GeritScraper.Common;
using GeritScraper.DataAdapter;
using GeritScraper.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

public class Program
{
    static async Task Main(string[] args)
    {
        var mongoDbAdapter = new MongoInstitutionDataAdapter("mongodb+srv://AdminLu:&%406TmNYccF4k24iJ6kCh@academicjobs.lgwya1x.mongodb.net/?retryWrites=true&w=majority", "Institutions", "institutionCollection");

        
        string inputPath = "C:\\Users\\g4m3r\\source\\repos\\GeritScraper\\GeritScraper\\bin\\Debug\\net6.0\\Output_new";
        
        // Get all Json Files (one file represents one institution)
        string [] filePaths = Directory.GetFiles(inputPath, "*.json", SearchOption.AllDirectories);

        var allInstitutes = new List<Institution>();

        foreach (var path in filePaths)
        {
            try
            {
                var institute = DeserializeSingleInstituteFromFile(path);
                var url = $"https://www.gerit.org/de/institutiondetail/{institute.Id}";
                await Console.Out.WriteLineAsync($"Found {institute.Name.De}, ID: {institute.Id}, Url: {url}"); // TODO log to file
                
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
        Console.WriteLine($"{allInstitutesInDb.Count} Institutions in DB.");
        Console.WriteLine("Ended");
    }

    static async Task LoopTreeChildren(List<ChildrenItem> children)
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
            var jsonString = await ScraperService.ScrapeJsonStringFromUrlAsync(url);
            var institutionDetails = DeserializeSingleInstitute(jsonString);

            // Clear Tree to reduce duplicate data
            institutionDetails.Tree = null;
            
            child.InstitutionDetails = institutionDetails;
        }
        catch (Exception e)
        {
            // TODO log to file
            Console.WriteLine(e.Message);
        }
    }

    static Institution DeserializeSingleInstituteFromFile(string jsonFilePath)
    {
        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException();
        }
        
        // Read the content of the file
        string jsonContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

        return DeserializeSingleInstitute(jsonContent);
    }
    
    static Institution DeserializeSingleInstitute(string jsonContent)
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
            Console.WriteLine($"An error happened while deserialization: {e}");
            throw;
        }
    }
}