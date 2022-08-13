namespace Wordlp.Models;

public record SavedGame
(
    List<Guess> Guesses,
    Word Solution // TODO: Encrypt or somethin
);