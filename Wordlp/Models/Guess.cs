using Wordlp.Enums;

namespace Wordlp.Models;

public record Guess(List<GuessedLetter> Letters)
{
    public int Length => Letters.Count;

    public bool Contains(char letter)
    {
        return Letters.Any(l => l.Letter.Value == letter);
    }

    public MatchTypes GetResultAt(int index)
    {
        return Letters.ElementAt(index).Result;
    }

    public bool IsWin()
    {
        return Letters.All(l => l.Result == MatchTypes.Exact);
    }
}

public record GuessedLetter(Letter Letter, MatchTypes Result);