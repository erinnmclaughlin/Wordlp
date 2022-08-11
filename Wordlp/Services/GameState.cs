namespace Wordlp.Services;

public class GameState
{
    public event EventHandler? OnGameStart;
    public event EventHandler? OnGameOver;
    public event EventHandler? OnInvalidSubmit;
    public event EventHandler? OnValidSubmit;

    public void NotifyGameStarted()
    {
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }

    public void NotifyGameOver()
    {
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public void NotifyInvalidSubmit()
    {
        OnInvalidSubmit?.Invoke(this, EventArgs.Empty);
    }

    public void NotifyValidSubmit()
    {
        OnValidSubmit?.Invoke(this, EventArgs.Empty);
    }
}
