using Wordlp.Enums;

namespace Wordlp.Models;

public record GuessedWord(List<GuessedLetter> Letters)
{
    public int Length => Letters.Count;

    public bool Contains(char letter)
    {
        return Letters.Any(l => l.Letter.Value == letter);
    }

    public MatchTypes GetResultAt(int index)
    {
        return Letters.ElementAt(index).MatchType;
    }

    public bool IsWin()
    {
        return Letters.All(l => l.MatchType == MatchTypes.Exact);
    }
}