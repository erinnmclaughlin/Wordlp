﻿using Wordlp.Models;
using Wordlp.Shared.Settings;

namespace Wordlp.Services;

public class Game
{
    public event EventHandler? OnGameOver;
    public event EventHandler? OnGameStart;
    public event EventHandler? OnInvalidSubmit;
    public event EventHandler? OnValidSubmit;

    public bool IsGameOver => Guesses.Count == GameSettings.MaxGuesses || IsWin;
    public bool IsWin => Guesses.Any(guess => guess.IsWin());

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

    private WordService WordService { get; }

    public Game(WordService wordService)
    {
        WordService = wordService;
    }

    public void GameOver()
    {
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public Guess? GetGuessByIndex(int index)
    {
        return Guesses.ElementAtOrDefault(index);
    }

    public GuessResult GetLetterResult(char letter)
    {
        var letters = Guesses.SelectMany(g => g.Letters.Where(l => l.Value == letter));

        if (letters.Any(l => l.Result == GuessResult.Match))
            return GuessResult.Match;

        if (letters.Any(l => l.Result == GuessResult.Contains))
            return GuessResult.Contains;

        return GuessResult.None;
    }

    public bool HasBeenGuessed(char letter)
    {
        return Guesses.Any(g => g.Contains(letter));
    }

    public bool IsCurrentGuess(int guessNumber)
    {
        return Guesses.Count == guessNumber;
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
        if (IsGameOver) return;

        if (!WordService.IsValid(CurrentGuess))
        {
            OnInvalidSubmit?.Invoke(this, EventArgs.Empty);
            return;
        }

        SubmitValidGuess();
    }

    public GuessResult VerifyLetterPosition(char letter, int index)
    {
        if (Solution.Value[index] == letter)
            return GuessResult.Match;

        if (Solution.Value.Contains(letter))
            return GuessResult.Contains;

        return GuessResult.None;
    }

    private void SubmitValidGuess()
    {
        List<GuessedLetter> letters = new();

        for (int i = 0; i < CurrentGuess.Length; i++)
        {
            var letter = CurrentGuess[i];

            /* Exact match */
            if (Solution.ContainsAtIndex(letter, i))
            {
                letters.Add(new GuessedLetter(letter, GuessResult.Match));
                continue;
            }

            /* No match */
            if (!Solution.Contains(letter))
            {
                letters.Add(new GuessedLetter(letter, GuessResult.None));
                continue;
            }

            /* Partial match */
            letters.Add(new GuessedLetter(letter, GuessResult.Contains));
        }

        CurrentGuess = string.Empty;
        Guesses.Add(new Guess(letters));
        OnValidSubmit?.Invoke(this, EventArgs.Empty);

        if (IsGameOver)
            GameOver();
    }
}
