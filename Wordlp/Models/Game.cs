namespace Wordlp.Models;

public class Game
{
    public const int MaxGuesses = 6;
    public const int WordLength = 5;

    private ValidWords ValidWords { get; set; }
    private WordCollection Words { get; set; }

    public List<Guess> Guesses { get; set; }
    public Word Word { get; set; }

    public int CurrentGuess => Guesses.Count;
    public int RemainingGuesses => MaxGuesses - CurrentGuess;

    public Game(ValidWords validWords, WordCollection words)
    {
        ValidWords = validWords;
        Words = words;

        Guesses = new List<Guess>();
        Word = Words.GetRandomWord();
    }

    public Guess? GetGuessByIndex(int index)
    {
        return Guesses.ElementAtOrDefault(index);
    }

    public bool HasBeenGuessed(char letter)
    {
        return Guesses.Any(g => g.Contains(letter));
    }

    public GuessResult GetLetterResult(char letter)
    {
        var letters = Guesses.SelectMany(g => g.Letters.Where(l => l.Value == letter));

        if (letters.Any(l => l.Result == GuessResult.Match))
            return GuessResult.Match;

        if (letters.Any(l => l.Result == GuessResult.Contains))
            return GuessResult.Contains;

        return GuessResult.None;
    }

    public bool IsValidGuess(string word)
    {
        return word.Length == 5 && ValidWords.Contains(word, StringComparer.InvariantCultureIgnoreCase);
    }

    public void SubmitGuess(string word)
    {
        List<GuessedLetter> letters = new();

        for (int i = 0; i < word.Length; i++)
        {
            var letter = word[i];
            letters.Add(new GuessedLetter(letter, VerifyLetterPosition(letter, i)));
        }

        Guesses.Add(new Guess(letters));
    }

    public GuessResult VerifyLetterPosition(char letter, int index)
    {
        if (Word.Value[index] == letter)
            return GuessResult.Match;

        if (Word.Value.Contains(letter))
            return GuessResult.Contains;

        return GuessResult.None;
    }
}
