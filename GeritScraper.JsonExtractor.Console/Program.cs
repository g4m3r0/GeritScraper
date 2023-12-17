using GeritScraper.JsonExtractor;

public class Program
{
    private static async Task Main(string[] args)
    {
        // Test for Json extractor
        var inputPath =
            "C:\\Users\\g4m3r\\source\\repos\\GeritScraper\\GeritScraper\\bin\\Debug\\net6.0\\Output_new";

        var extractor = new JsonExtractor();
        await extractor.RunExtractor(inputPath);

        Console.ReadLine();
    }
}