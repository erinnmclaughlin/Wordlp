namespace Wordlp.Services;

public class Game
{
    public event EventHandler? OnGameOver;
    public event EventHandler? OnGameStart;
    public event EventHandler? OnInvalidSubmit;

    public bool IsGameOver { get; private set; }

    public void GameOver()
    {
        IsGameOver = true;
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public void NewGame()
    {
        IsGameOver = false;
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }

    public void NotifyInvalidSubmit()
    {
        OnInvalidSubmit?.Invoke(this, EventArgs.Empty);
    }
}
