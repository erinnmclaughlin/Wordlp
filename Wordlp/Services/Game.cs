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

    private WordService WordService { get; }

    public Game(WordService wordService)
    {
        WordService = wordService;
    }

    public void GameOver()
    {
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public async Task NewGame()
    {
        Guesses = new();
        Solution = await WordService.GetRandomWord();
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }

    public void Submit(string guess)
    {
        if (IsGameOver) return;

        if (!WordService.IsValid(guess))
        {
            OnInvalidSubmit?.Invoke(this, EventArgs.Empty);
            return;
        }

        SubmitValidGuess(guess);
    }

    private void SubmitValidGuess(string guess)
    {
        List<GuessedLetter> letters = new();

        for (int i = 0; i < guess.Length; i++)
        {
            var letter = guess[i];

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
            if (Solution.GetLetters().Any(l => l.Value == letter && guess[l.Index] != l.Value))
            {
                letters.Add(new GuessedLetter(letter, GuessResult.Contains));
                continue;
            }

            letters.Add(new GuessedLetter(letter, GuessResult.None));
        }

        Guesses.Add(new Guess(letters));
        OnValidSubmit?.Invoke(this, EventArgs.Empty);

        if (IsGameOver)
            GameOver();
    }
}
