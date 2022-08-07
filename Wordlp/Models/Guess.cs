namespace Wordlp.Models;

public record Guess(List<GuessedLetter> Letters)
{
    public int Length => Letters.Count;

    public bool Contains(char letter)
    {
        return Letters.Any(l => l.Value == letter);
    }

    public GuessResult GetResultAt(int index)
    {
        return Letters.ElementAt(index).Result;
    }

    public bool IsWin()
    {
        return Letters.All(l => l.Result == GuessResult.Match);
    }
}

public record GuessedLetter(char Value, GuessResult Result);