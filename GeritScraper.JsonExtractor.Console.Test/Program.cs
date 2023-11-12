using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

public class Program
{
    static async Task Main(string[] args)
    {
        string inputPath = "C:\\Users\\g4m3r\\source\\repos\\GeritScraper\\GeritScraper\\bin\\Debug\\net6.0\\Output_new";
        
        // Get all Json Files (one file represents one institution)
        string [] filePaths = Directory.GetFiles(inputPath, "*.json", SearchOption.AllDirectories);

        var allInstitutes = new List<Institution>();

        foreach (var path in filePaths)
        {
            try
            {
                var institute = DeserializeSingleInstitute(path);
                allInstitutes.Add(institute);
                await Console.Out.WriteLineAsync($"Deserializes {institute.Name.De}");
            }
            catch (Exception e)
            {
                throw;
                // Ignore for now
            }
        }

        Console.WriteLine("Ended");
        Console.ReadLine();
    }

    static Institution DeserializeSingleInstitute(string jsonFilePath)
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
        
        // Read the content of the file
        string jsonContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

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