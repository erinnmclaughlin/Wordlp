using Wordlp.Enums;

namespace Wordlp.Models;

public record GuessedLetter(Letter Letter, MatchTypes MatchType);