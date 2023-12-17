using GeritScraper.DataModels;

namespace GeritScraper.JsonExtractor;

public class FindMatch
{
    public static async Task<ChildrenItem> FindBestUrlMatch(List<ChildrenItem> children, string searchUrl)
    {
        var state = new MatchState();

        await LoopTreeChildren(children, searchUrl, state);

        return state.BestMatch;
    }

    public static async Task<ChildrenItem> FindBestUrlMatchAcrossInstitution(List<Institution> institutions,
        string searchUrl)
    {
        var state = new MatchState();

        foreach (var institute in institutions)
        {
            if (institute.Tree is null) continue;

            await LoopTreeChildren(institute.Tree.Children, searchUrl, state);
        }

        return state.BestMatch;
    }

    private static async Task LoopTreeChildren(List<ChildrenItem> children, string searchUrl, MatchState state)
    {
        if (children == null || !children.Any())
            return;

        foreach (var child in children)
        {
            if (child.InstitutionDetails is null) continue;

            var matchLength = GetMatchLength(child.InstitutionDetails.Url, searchUrl);

            if (matchLength > state.MaxMatchLength)
            {
                state.MaxMatchLength = matchLength;
                state.BestMatch = child;
            }

            await LoopTreeChildren(child.Children, searchUrl, state);
        }
    }

    private static int GetMatchLength(string url1, string url2)
    {
        if (string.IsNullOrEmpty(url1) || string.IsNullOrEmpty(url2))
            return 0;

        var matchLength = 0;
        var minLength = Math.Min(url1.Length, url2.Length);

        for (var i = 0; i < minLength; i++)
            if (url1[i] == url2[i])
                matchLength++;
            else
                break;
        return matchLength;
    }
}

public class MatchState
{
    public ChildrenItem? BestMatch { get; set; }
    public int MaxMatchLength { get; set; }
}