using Wordlp.Models;

namespace Wordlp.Services;

public class GameService
{   
    private GameState GameState { get; }
    private PlayerHistoryService PlayerHistory { get; }
    private ValidWords ValidWords { get; }
    private WordCollection Words { get; }

    public List<Guess> Guesses { get; private set; } = new();
    public Word Word { get; private set; } = null!;

    public int CurrentGuess => Guesses.Count;
    public bool IsGameOver { get; private set; }
    public int RemainingGuesses => GameSettings.MaxGuesses - CurrentGuess;

    public GameService(GameState gameState, PlayerHistoryService playerHistory, ValidWords validWords, WordCollection words)
    {
        GameState = gameState;
        PlayerHistory = playerHistory;
        ValidWords = validWords;
        Words = words;
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

    public async Task StartNewGame()
    {
        Guesses = new();
        IsGameOver = false;

        await GetNewWord();
        GameState.NotifyGameStarted();
    }

    public async Task SubmitGuess(string guessedWord)
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
            if (Word.GetLetters().Any(l => l.Value == letter && guessedWord[l.Index] != l.Value))
            {
                letters.Add(new GuessedLetter(letter, GuessResult.Contains));
                continue;
            }

            letters.Add(new GuessedLetter(letter, GuessResult.None));
        }

        var guess = new Guess(letters);
        Guesses.Add(guess);

        if (Guesses.Count == GameSettings.MaxGuesses || guess.IsWin())
            await GameOver();
    }

    public async Task GameOver()
    {
        IsGameOver = true;
        await PlayerHistory.SubmitAttempt(Word, Guesses.Count);
    }

    public GuessResult VerifyLetterPosition(char letter, int index)
    {
        if (Word.Value[index] == letter)
            return GuessResult.Match;

        if (Word.Value.Contains(letter))
            return GuessResult.Contains;

        return GuessResult.None;
    }

    private async Task GetNewWord()
    {
        var guessedWords = await PlayerHistory.GetHistory();
        var unguessedWords = Words.Words.Where(w => !guessedWords.Any(g => g.Word.Value == w.Value));

        if (unguessedWords.Any() == false)
            unguessedWords = Words.Words;

        var randomIndex = new Random().Next(0, unguessedWords.Count() - 1);
        var word = unguessedWords.ElementAt(randomIndex);

        Console.WriteLine(word.Value);

        Word = word;
    }
}
