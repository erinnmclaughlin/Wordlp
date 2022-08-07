namespace Wordlp.Models;

public record GameResult
{
    public int AttemptCount { get; set; }
    public Word Word { get; init; }

    public GameResult(Word word, int attemptCount)
    {
        AttemptCount = attemptCount;
        Word = word;
    }
}