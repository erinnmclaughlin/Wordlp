namespace Wordlp.Models;

public record SavedGame
(
    List<GuessedWord> Guesses,
    Word Solution // TODO: Encrypt or somethin
);