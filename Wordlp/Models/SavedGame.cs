namespace Wordlp.Models;

public record SavedGame
(
    string CurrentGuess,
    List<Guess> Guesses,
    Word Solution // TODO: Encrypt or somethin
);