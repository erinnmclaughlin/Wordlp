using Wordlp.Enums;
using Wordlp.Models;

namespace Wordlp.Services;

public static class GuessAnalyzer
{
    public static GuessedWord Analyze(string guess, string solution)
    {
        return new GuessedWord(GetGuessedLetters(guess, solution).OrderBy(gl => gl.Letter.Index).ToList());
    }

    private static IEnumerable<GuessedLetter> GetGuessedLetters(string guess, string solution)
    {
        var guessedLetters = guess.GetLetters().ToList();
        var solutionLetters = solution.GetLetters().ToList();

        foreach (var letterGroup in guessedLetters.GroupBy(l => l.Value))
        {
            var matchingLetters = solutionLetters.Where(l => l.Value == letterGroup.Key).ToList();
            var exactMatches = letterGroup.Where(l => matchingLetters.Any(m => m.Index == l.Index));

            foreach (var match in exactMatches)
                yield return new GuessedLetter(match, MatchTypes.Exact);

            var remainingLetters = matchingLetters.Count - exactMatches.Count();

            foreach (var letter in letterGroup.Where(l => !exactMatches.Contains(l)))
            {
                var matchType = MatchTypes.None;

                if (remainingLetters > 0)
                    matchType = matchingLetters.Any(l => l.Index == letter.Index) ? MatchTypes.Exact : MatchTypes.Partial;

                yield return new GuessedLetter(letter, matchType);

                remainingLetters--;
            }
        }
    }
}
