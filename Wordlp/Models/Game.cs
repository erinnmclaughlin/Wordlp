using Wordlp.Enums;
using Wordlp.Models.Settings;
using Wordlp.Services;

namespace Wordlp.Models;

public class Game
{
    public event EventHandler? OnGameOver;
    public event EventHandler? OnGameStart;
    public event EventHandler? OnInvalidSubmit;
    public event EventHandler? OnValidSubmit;

    public Guess CurrentGuess { get; set; } = new();
    public List<GuessedWord> Guesses { get; private set; } = new();
    public Word Solution { get; private set; } = null!;

    private IWordService WordService { get; }

    public Game(IWordService wordService)
    {
        WordService = wordService;
    }

    public bool IsGameOver()
    {
        return Guesses.Count == GameSettings.MaxGuesses || IsWin();
    }

    public bool IsWin()
    {
        return Guesses.Any(guess => guess.IsWin());
    }

    public void LoadGame(SavedGame savedGame)
    {
        CurrentGuess = new();
        Guesses = savedGame.Guesses;
        Solution = savedGame.Solution;
    }

    public async Task NewGame()
    {
        CurrentGuess = new();
        Guesses = new();
        Solution = await WordService.GetRandomWord();
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }

    public void SubmitGuess()
    {
        if (IsGameOver()) return;

        if (!WordService.IsValid(CurrentGuess.Value))
        {
            OnInvalidSubmit?.Invoke(this, EventArgs.Empty);
            return;
        }

        SubmitValidGuess();
    }

    private void SubmitValidGuess()
    {
        var results = new List<GuessedLetter>();

        foreach (var guessGroup in CurrentGuess.Letters.GroupBy(l => l.Value))
        {
            var guessedLetter = guessGroup.Key;
            var guessedLetters = guessGroup.ToList();

            var matchingLetters = Solution.Letters.Where(l => l.Value == guessedLetter).ToList();
            if (!matchingLetters.Any())
            {
                results.AddRange(guessedLetters.Select(l => new GuessedLetter(l, MatchTypes.None)));
                continue;
            }

            var exactMatches = guessedLetters.Where(letter => matchingLetters.Any(match => match.Index == letter.Index)).ToList();
            results.AddRange(exactMatches.Select(letter => new GuessedLetter(letter, MatchTypes.Exact)));
            guessedLetters.RemoveAll(letter => matchingLetters.Any(match => match.Index == letter.Index));

            var remainingMatchCount = matchingLetters.Count - exactMatches.Count;

            results.AddRange(guessedLetters.Take(remainingMatchCount).Select(l => new GuessedLetter(l, MatchTypes.Partial)));
            results.AddRange(guessedLetters.Skip(remainingMatchCount).Select(l => new GuessedLetter(l, MatchTypes.None)));
        }

        CurrentGuess = new();
        Guesses.Add(new GuessedWord(results.OrderBy(r => r.Letter.Index).ToList()));
        OnValidSubmit?.Invoke(this, EventArgs.Empty);

        if (IsGameOver())
            OnGameOver?.Invoke(this, EventArgs.Empty);
    }
}
