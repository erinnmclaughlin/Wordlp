namespace Wordlp.Models;

public record GameResult
{
    public int AttemptCount { get; set; }
    public Word Word { get; init; }
    public bool IsWin { get; set; }

    public GameResult(Word word, int attemptCount, bool isWin)
    {
        AttemptCount = attemptCount;
        Word = word;
        IsWin = isWin;
    }
}