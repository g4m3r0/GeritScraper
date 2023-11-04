using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

public class Program
{
    static async Task Main(string[] args)
    {
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
        
        // Path to your JSON file
        string jsonFilePath = "C:\\Users\\g4m3r\\source\\repos\\GeritScraper\\GeritScraper\\bin\\Debug\\net6.0\\Output\\Technische Universität Chemnitz\\10355.json";

        // Read the content of the file
        string jsonContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

        // Deserialize the JSON content to the RootObject class
        // Replace 'RootObject' with the actual name of your root class
        try
        {
            var deserializedData = JsonConvert.DeserializeObject<RootObject>(jsonContent, jsonSettings);
            
            var institution = deserializedData.Institution;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        Console.ReadLine();
    }
}