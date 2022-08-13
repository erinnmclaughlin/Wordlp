using Blazored.LocalStorage;
using Wordlp.Models;
using Wordlp.Shared.Settings;

namespace Wordlp.Services;

public class Game
{
    public event EventHandler? OnGameOver;
    public event EventHandler? OnGameStart;
    public event EventHandler? OnInvalidSubmit;
    public event EventHandler? OnValidSubmit;

    public bool IsGameOver => Guesses.Count == GameSettings.MaxGuesses || Guesses.Any(guess => guess.IsWin());
    public List<Guess> Guesses { get; private set; } = new();
    public Word Solution { get; private set; } = null!;

    private string _currentGuess = string.Empty;
    public string CurrentGuess
    {
        get => _currentGuess;
        set
        {
            if (IsGameOver)
            {
                _currentGuess = string.Empty;
                return;
            }

            if (value.Length <= GameSettings.WordLength)
                _currentGuess = value;
        }
    }

    private ILocalStorageService LocalStorage { get; }
    private WordService WordService { get; }

    public Game(ILocalStorageService localStorage, WordService wordService)
    {
        LocalStorage = localStorage;
        WordService = wordService;
    }

    public void GameOver()
    {
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public async Task Load()
    {
        var savedGame = await LocalStorage.GetItemAsync<SavedGame>(LocalStorageSettings.Keys.SavedGame);

        if (savedGame != null)
        {
            CurrentGuess = savedGame.CurrentGuess;
            Guesses = savedGame.Guesses;
            Solution = savedGame.Solution;
            return;
        }

        await NewGame();
    }

    public async Task NewGame()
    {
        CurrentGuess = string.Empty;
        Guesses = new();
        Solution = await WordService.GetRandomWord();
        OnGameStart?.Invoke(this, EventArgs.Empty);

        await SaveGame();
    }

    public async Task SaveGame()
    {
        var savedGame = new SavedGame(CurrentGuess, Guesses, Solution);
        await LocalStorage.SetItemAsync(LocalStorageSettings.Keys.SavedGame, savedGame);
    }

    public void Submit()
    {
        if (IsGameOver) return;

        if (!WordService.IsValid(CurrentGuess))
        {
            OnInvalidSubmit?.Invoke(this, EventArgs.Empty);
            return;
        }

        SubmitValidGuess();
    }

    private async void SubmitValidGuess()
    {
        List<GuessedLetter> letters = new();

        for (int i = 0; i < CurrentGuess.Length; i++)
        {
            var letter = CurrentGuess[i];

            /* No match */
            if (!Solution.Value.Contains(letter))
            {
                letters.Add(new GuessedLetter(letter, GuessResult.None));
                continue;
            }

            /* Exact match */
            if (Solution.Value[i] == letter)
            {
                letters.Add(new GuessedLetter(letter, GuessResult.Match));
                continue;
            }

            /* Partial match */
            if (Solution.GetLetters().Any(l => l.Value == letter && CurrentGuess[l.Index] != l.Value))
            {
                letters.Add(new GuessedLetter(letter, GuessResult.Contains));
                continue;
            }

            letters.Add(new GuessedLetter(letter, GuessResult.None));
        }

        CurrentGuess = string.Empty;
        Guesses.Add(new Guess(letters));
        OnValidSubmit?.Invoke(this, EventArgs.Empty);

        if (IsGameOver)
            GameOver();

        await SaveGame();
    }
}
