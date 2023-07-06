namespace GeritScraper.Common;

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

public class HtmlScraper
{
    private readonly HttpClient Client = new HttpClient();
    private readonly HtmlToText htmlToText = new HtmlToText();

    public HtmlScraper()
    {
         this.Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
    }

    public async Task ScrapeAsync(string inputFolder, string failedUrlLogPath, bool convertToText = false, int maxConcurrentTasks = 10)
    {
        if (string.IsNullOrEmpty(inputFolder))
            inputFolder = "C:\\Users\\g4m3r\\source\\repos\\GeritScraper\\GeritScraper\\bin\\Debug\\net6.0\\Output";

        if (string.IsNullOrEmpty(failedUrlLogPath))
            failedUrlLogPath = "C:\\Users\\g4m3r\\source\\repos\\GeritScraper\\GeritScraper\\bin\\Debug\\net6.0\\Output\\failed_urls.txt";

        var directories = Directory.GetDirectories(inputFolder);

        SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrentTasks, maxConcurrentTasks);
        var tasks = directories.Select(async dir =>
        {
            await semaphore.WaitAsync();
            try
            {
                await ProcessDirectory(dir, failedUrlLogPath, convertToText);
            }
            finally
            {
                semaphore.Release();
            }
        }).ToArray();

        await Task.WhenAll(tasks);
    }

    private async Task ProcessDirectory(string childFolder, string failedUrlLogPath, bool convertToText)
    {
        await Console.Out.WriteLineAsync($"Processing folder: {childFolder}");

        var jsonFilePath = Path.Combine(childFolder, "institutes.json");
        if (!File.Exists(jsonFilePath))
            return;

        var json = await File.ReadAllTextAsync(jsonFilePath);
        var jsonArray = JArray.Parse(json);
        int totalInstitutes = jsonArray.Count;

        for (int j = 0; j < totalInstitutes; j++)
        {
            var institute = (JObject)jsonArray[j];
            var id = institute["Id"].ToString();
            var name = institute["Name"].ToString();
            var url = institute["Url"].ToString();

            await Console.Out.WriteLineAsync($"  Processing institute {j + 1}/{totalInstitutes}:\n  {name} ({url})");

            try
            {
                var html = await this.ScrapeSingleUrlAsync(url);
                var htmlFileName = $"{id}_{name}.html";
                var htmlFilePath = Path.Combine(childFolder, htmlFileName);

                await File.WriteAllTextAsync(htmlFilePath, html);

                if (convertToText)
                {
                    var text = this.htmlToText.GetTextFromHtml(html);
                    await File.WriteAllTextAsync(htmlFilePath.Replace(".html", ".txt"), text);
                }

            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync($"  Failed to load URL: {url}.\n   Error: {e.Message}");
                await File.AppendAllTextAsync(failedUrlLogPath, $"{{\"Url\": \"{url}\", \"Error\": \"{e.Message}\"}},\n");
            }
        }
    }

    public async Task<string> ScrapeSingleUrlAsync(string url)
    {
        var html = await this.Client.GetStringAsync(url);
        return html;
    }
}