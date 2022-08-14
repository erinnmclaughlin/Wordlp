namespace Wordlp.Models;

public record Guess(List<GuessedLetter> Letters)
{
    public int Length => Letters.Count;

    public bool Contains(char letter)
    {
        return Letters.Any(l => l.Letter.Value == letter);
    }

    public LetterMatchType GetResultAt(int index)
    {
        return Letters.ElementAt(index).Result;
    }

    public bool IsWin()
    {
        return Letters.All(l => l.Result == LetterMatchType.Exact);
    }
}

public record GuessedLetter(Letter Letter, LetterMatchType Result);