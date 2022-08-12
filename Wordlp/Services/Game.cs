using Wordlp.Models;

namespace Wordlp.Services;

public class Game
{
    public event EventHandler? OnGameOver;
    public event EventHandler? OnGameStart;
    public event EventHandler? OnInvalidSubmit;

    public bool IsGameOver { get; private set; }
    public List<Guess> Guesses { get; private set; } = new();
    public string Guess { get; set; } = string.Empty;
    public Word Solution { get; private set; } = null!;

    private WordService WordService { get; }

    public Game(WordService wordService)
    {
        WordService = wordService;
    }

    public void GameOver()
    {
        IsGameOver = true;
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public async Task NewGame()
    {
        Guesses = new();
        Guess = string.Empty;
        Solution = await WordService.GetRandomWord();

        IsGameOver = false;
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }

    public void NotifyInvalidSubmit()
    {
        OnInvalidSubmit?.Invoke(this, EventArgs.Empty);
    }
}
