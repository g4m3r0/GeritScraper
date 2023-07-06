namespace GeritScraper.InstituteHtml
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using GeritScraper.Common;
    using Newtonsoft.Json.Linq;

    class Program
    {

        static async Task Main(string[] args)
        {
            string inputFolder = "C:\\Users\\g4m3r\\source\\repos\\GeritScraper\\GeritScraper\\bin\\Debug\\net6.0\\Output";
            string failedUrlsFilePath = "C:\\Users\\g4m3r\\source\\repos\\GeritScraper\\GeritScraper\\bin\\Debug\\net6.0\\Output\\failed_urls.txt";

            //var scraper = new HtmlScraper();
            //await scraper.ScrapeAsync(inputFolder, failedUrlsFilePath, true);

            var nlConverter = new NaturalLanguageConverter();
            await nlConverter.ProcessTextFiles(inputFolder);
        }
    }
}