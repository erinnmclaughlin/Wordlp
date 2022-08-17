using Wordlp.Models.Settings;
using Wordlp.Services;

namespace Wordlp.Models;

public class Game
{
    public event EventHandler? OnGameOver;
    public event EventHandler? OnGameStart;
    public event EventHandler? OnInvalidSubmit;
    public event EventHandler? OnValidSubmit;

    private string _currentGuess = string.Empty;
    public string CurrentGuess
    {
        get => _currentGuess;
        set
        {
            if (IsGameOver())
            {
                _currentGuess = string.Empty;
                return;
            }

            if (value.Length <= GameSettings.WordLength)
                _currentGuess = value;
        }
    }
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
        CurrentGuess = string.Empty;
        Guesses = savedGame.Guesses;
        Solution = savedGame.Solution;
    }

    public async Task NewGame()
    {
        CurrentGuess = string.Empty;
        Guesses = new();
        Solution = await WordService.GetRandomWord();
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }

    public void SubmitGuess()
    {
        if (IsGameOver()) return;

        if (!WordService.IsValid(CurrentGuess))
        {
            OnInvalidSubmit?.Invoke(this, EventArgs.Empty);
            return;
        }

        SubmitValidGuess();
    }

    private void SubmitValidGuess()
    {
        var analyzedGuess = GuessAnalyzer.Analyze(CurrentGuess, Solution.Value);
       
        CurrentGuess = string.Empty;
        Guesses.Add(analyzedGuess);

        OnValidSubmit?.Invoke(this, EventArgs.Empty);

        if (IsGameOver())
            OnGameOver?.Invoke(this, EventArgs.Empty);
    }
}
