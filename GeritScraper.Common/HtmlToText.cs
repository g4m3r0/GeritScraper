namespace GeritScraper.Common;

using System.Runtime.CompilerServices;
using Textify;

public class HtmlToText
{
    public HtmlToTextConverter Converter { get; set; }

    public HtmlToText()
    {
        this.Converter = new HtmlToTextConverter();
    }

    public string GetTextFromHtml(string html)
    {
        string output = this.Converter.Convert(html);
        return output;
    }

}
