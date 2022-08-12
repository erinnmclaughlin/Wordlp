namespace Wordlp.Services;

public class Game
{
    public event EventHandler? OnGameOver;
    public event EventHandler? OnGameStart;
    public event EventHandler? OnInvalidSubmit;

    public void NotifyGameOver()
    {
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public void NotifyGameStart()
    {
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }

    public void NotifyInvalidSubmit()
    {
        OnInvalidSubmit?.Invoke(this, EventArgs.Empty);
    }
}
