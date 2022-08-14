using Wordlp.Models;
using Wordlp.Models.Settings;

namespace Wordlp.Services;

public class Game
{
    public event EventHandler? OnGameOver;
    public event EventHandler? OnGameStart;
    public event EventHandler? OnInvalidSubmit;
    public event EventHandler? OnValidSubmit;

    public List<Guess> Guesses { get; private set; } = new();
    public Word Solution { get; private set; } = null!;

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

    private IWordService WordService { get; }

    public Game(IWordService wordService)
    {
        WordService = wordService;
    }

    public Guess? GetGuessByIndex(int index)
    {
        return Guesses.ElementAtOrDefault(index);
    }

    public Models.LetterMatchType GetLetterResult(char letter)
    {
        var letters = Guesses.SelectMany(g => g.Letters.Where(l => l.Letter.Value == letter));

        if (letters.Any(l => l.Result == Models.LetterMatchType.Exact))
            return Models.LetterMatchType.Exact;

        if (letters.Any(l => l.Result == Models.LetterMatchType.Partial))
            return Models.LetterMatchType.Partial;

        return Models.LetterMatchType.None;
    }

    public bool HasBeenGuessed(char letter)
    {
        return Guesses.Any(g => g.Contains(letter));
    }

    public bool IsCurrentGuess(int guessNumber)
    {
        return Guesses.Count == guessNumber;
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

    public void Submit()
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
        var letters = CurrentGuess.GetLetters();
        var results = new List<GuessedLetter>();

        foreach (var letterGroup in letters.GroupBy(l => l.Value))
        {
            var matchingLetters = Solution.Letters.Where(l => l.Value == letterGroup.Key);

            if (!matchingLetters.Any())
            {
                results.AddRange(letterGroup.Select(letter => new GuessedLetter(letter, LetterMatchType.None)));
                continue;
            }

            var exactMatches = letterGroup.Where(letter => matchingLetters.Any(match => match.Index == letter.Index));
            results.AddRange(exactMatches.Select(letter => new GuessedLetter(letter, LetterMatchType.Exact)));

            var remainingLetterCount = matchingLetters.Count() - exactMatches.Count();
            var partialMatches = letterGroup.Where(letter => !matchingLetters.Any(match => match.Index == letter.Index));

            results.AddRange(partialMatches.Take(remainingLetterCount).Select(letter => new GuessedLetter(letter, LetterMatchType.Partial)));
            results.AddRange(partialMatches.Skip(remainingLetterCount).Select(letter => new GuessedLetter(letter, LetterMatchType.None)));
        }

        CurrentGuess = string.Empty;
        Guesses.Add(new Guess(results));
        OnValidSubmit?.Invoke(this, EventArgs.Empty);

        if (IsGameOver())
            OnGameOver?.Invoke(this, EventArgs.Empty);
    }
}
