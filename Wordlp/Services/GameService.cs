using Blazored.LocalStorage;
using Wordlp.Models;

namespace Wordlp.Services;

public class GameService
{
    public const int MaxGuesses = 6;
    public const int WordLength = 5;

    private ILocalStorageService LocalStorage { get; set; }
    private ValidWords ValidWords { get; set; }
    private WordCollection Words { get; set; }

    public List<Guess> Guesses { get; set; } = new();
    public Word Word { get; set; } = null!;

    public int CurrentGuess => Guesses.Count;
    public bool GameOver { get; private set; }
    public int RemainingGuesses => MaxGuesses - CurrentGuess;

    public GameService(ILocalStorageService localStorage, ValidWords validWords, WordCollection words)
    {
        LocalStorage = localStorage;
        ValidWords = validWords;
        Words = words;

        StartNewGame();
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

    public void StartNewGame()
    {
        Guesses = new();
        Word = Words.GetRandomWord();
        GameOver = false;
    }

    public void SubmitGuess(string guessedWord)
    {
        List<GuessedLetter> letters = new();

        for (int i = 0; i < guessedWord.Length; i++)
        {
            var letter = guessedWord[i];

            /* No match */
            if (!Word.Value.Contains(letter))
            {
                letters.Add(new GuessedLetter(letter, GuessResult.None));
                continue;
            }

            /* Exact match */
            if (Word.Value[i] == letter)
            {
                letters.Add(new GuessedLetter(letter, GuessResult.Match));
                continue;
            }

            /* Partial match */
            if (Word.Letters.Any(l => l.Value == letter && guessedWord[l.Index] != l.Value))
            {
                letters.Add(new GuessedLetter(letter, GuessResult.Contains));
                continue;
            }

            letters.Add(new GuessedLetter(letter, GuessResult.None));
        }

        var guess = new Guess(letters);
        Guesses.Add(guess);

        if (Guesses.Count == MaxGuesses || guess.Letters.All(l => l.Result == GuessResult.Match))
            GameOver = true;
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
