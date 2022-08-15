using Wordlp.Enums;

namespace Wordlp.Models;

public record GuessedWord(List<GuessedLetter> Letters)
{
    public GuessedLetter this[int index] => Letters.First(l => l.Letter.Index == index);

    public bool Contains(char letter)
    {
        return Letters.Any(l => l.Letter.Value == letter);
    }

    public bool IsWin()
    {
        return Letters.All(l => l.MatchType == MatchTypes.Exact);
    }
}