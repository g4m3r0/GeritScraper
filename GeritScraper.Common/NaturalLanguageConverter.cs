using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatCompletion.Common;
using Newtonsoft.Json.Linq;

namespace GeritScraper.Common;

public class NaturalLanguageConverter
{
    private ChatCompletion.Common.ChatCompletion chatBot = new ChatCompletion.Common.ChatCompletion();

    public string Prompt { get; set; }

    public NaturalLanguageConverter()
    {
        chatBot.Endpoint = "https://free.churchless.tech";

        // Load prompt template
        this.Prompt = File.ReadAllText("./Files/prompt.txt");
    }


    public string GetPrompt(string text)
    {
        return this.Prompt.Replace("[REPLACEWITHTEXT]", text);
    }


    public async Task<string> SendPrompt(string text)
    {
        var fullPrompt = this.GetPrompt(text);
        string response = await chatBot.CreateAsync(fullPrompt);
        return response;
    }

    public async Task ProcessTextFiles(string inputFolder)
    {
        var directories = Directory.GetDirectories(inputFolder);
        int totalDirectories = directories.Length;

        for (int i = 0; i < totalDirectories; i++)
        {
            var childFolder = directories[i];
            await Console.Out.WriteLineAsync($"Processing folder {i + 1}/{totalDirectories}: {childFolder}");

            var htmlFiles = Directory.GetFiles(childFolder, "*.txt");
            int totalHtmlFiles = htmlFiles.Length;

            for (int j = 0; j < totalHtmlFiles; j++)
            {
                var htmlFilePath = htmlFiles[j];
                await Console.Out.WriteLineAsync($"  Processing Text file {j + 1}/{totalHtmlFiles}: {htmlFilePath}");

                var htmlContent = await File.ReadAllTextAsync(htmlFilePath);

                // Assuming SendPrompt is an async method that takes a string and returns a string.
                // Replace with your actual method.
                var response = await SendPrompt(htmlContent);

                await Console.Out.WriteLineAsync($"Response:\n{response}\n");

                var responseFilePath = htmlFilePath.Replace(".txt", ".response.json");
                await File.WriteAllTextAsync(responseFilePath, response);
            }

            // Progress bar
            double progress = (double)(i + 1) / totalDirectories;
            DrawProgressBar(progress);
        }

        static void DrawProgressBar(double proportionComplete)
        {
            int totalChunks = 30;

            // draw filled part
            int filledChunks = (int)(totalChunks * proportionComplete);
            Console.Write("[");
            Console.Write(new string('=', filledChunks));

            // draw unfilled part
            int unfilledChunks = totalChunks - filledChunks;
            Console.Write(new string(' ', unfilledChunks));
            Console.Write("]");

            // draw percentages
            Console.Write($" {proportionComplete:P0}");
            Console.Write(Environment.NewLine);
        }
    }
}
