namespace Wordlp.Services;

public class GameState
{
    public event EventHandler? OnInvalidSubmit;

    public void NotifyInvalidSubmit()
    {
        OnInvalidSubmit?.Invoke(this, EventArgs.Empty);
    }
}
