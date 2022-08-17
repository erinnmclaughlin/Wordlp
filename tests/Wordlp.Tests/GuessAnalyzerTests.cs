using System.Collections;
using Wordlp.Enums;
using Wordlp.Services;

namespace Wordlp.Tests;

public class GuessAnalyzerTests
{
    [Theory]
    [ClassData(typeof(TestData))]
    public void RunTests(string solution, string guess, List<MatchTypes> expectedResults)
    {
        var actualResults = GuessAnalyzer.Analyze(guess, solution)
            .Letters
            .Select(l => l.MatchType);

        actualResults.Should().BeEquivalentTo(expectedResults);
    }

    public class TestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                "ABC",
                "ABA",
                new List<MatchTypes> { MatchTypes.Exact, MatchTypes.Exact, MatchTypes.None }
            };

            yield return new object[]
            {
                "ABC",
                "BAC",
                new List<MatchTypes> { MatchTypes.Partial, MatchTypes.Partial, MatchTypes.Exact }
            };

            yield return new object[]
            {
                "DRUMS",
                "CROSS",
                new List<MatchTypes> 
                { 
                    MatchTypes.None,
                    MatchTypes.Exact,
                    MatchTypes.None,
                    MatchTypes.None,
                    MatchTypes.Exact
                }
            };

            yield return new object[]
            {
                "CAMPY",
                "APPLE",
                new List<MatchTypes>
                {
                    MatchTypes.Partial,
                    MatchTypes.Partial,
                    MatchTypes.None,
                    MatchTypes.None,
                    MatchTypes.None
                }
            };

            yield return new object[]
            {
                "CAMPY",
                "HAPPY",
                new List<MatchTypes>
                {
                    MatchTypes.None,
                    MatchTypes.Exact,
                    MatchTypes.None,
                    MatchTypes.Exact,
                    MatchTypes.Exact
                }
            };

            yield return new object[]
            {
                "GAMES",
                "SALES",
                new List<MatchTypes>
                {
                    MatchTypes.None,
                    MatchTypes.Exact,
                    MatchTypes.None,
                    MatchTypes.Exact,
                    MatchTypes.Exact
                }
            };

            yield return new object[]
            {
                "GAMES",
                "GAMES",
                new List<MatchTypes>
                {
                    MatchTypes.Exact,
                    MatchTypes.Exact,
                    MatchTypes.Exact,
                    MatchTypes.Exact,
                    MatchTypes.Exact
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
