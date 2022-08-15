using Wordlp.Enums;
using Wordlp.Models;
using Wordlp.Services;

namespace Wordlp.Tests;

public class GameTests
{
    [Fact]
    public void SubmitStoresCorrectResult()
    {
        var solution = new Word("SWEET", "");
        var savedGame = new SavedGame(new(), solution);

        var game = new Game(new MockWordService());
        game.LoadGame(savedGame);

        game.CurrentGuess = "SALES";
        game.SubmitGuess();

        var lastGuess = game.Guesses.Last();

        Assert.Equal(MatchTypes.Exact, lastGuess.Letters.First(l => l.Letter.Index == 0).MatchType);
        Assert.Equal(MatchTypes.None, lastGuess.Letters.First(l => l.Letter.Index == 1).MatchType);
        Assert.Equal(MatchTypes.None, lastGuess.Letters.First(l => l.Letter.Index == 2).MatchType);
        Assert.Equal(MatchTypes.Exact, lastGuess.Letters.First(l => l.Letter.Index == 3).MatchType);
        Assert.Equal(MatchTypes.None, lastGuess.Letters.First(l => l.Letter.Index == 4).MatchType);
    }

    [Fact]
    public void SubmitStoresCorrectResult2()
    {
        var solution = new Word("YUMMY", "");
        var savedGame = new SavedGame(new(), solution);

        var game = new Game(new MockWordService());
        game.LoadGame(savedGame);

        game.CurrentGuess = "MUMMY";
        game.SubmitGuess();

        var lastGuess = game.Guesses.Last();

        Assert.Equal(MatchTypes.None, lastGuess.Letters.First(l => l.Letter.Index == 0).MatchType);
        Assert.Equal(MatchTypes.Exact, lastGuess.Letters.First(l => l.Letter.Index == 1).MatchType);
        Assert.Equal(MatchTypes.Exact, lastGuess.Letters.First(l => l.Letter.Index == 2).MatchType);
        Assert.Equal(MatchTypes.Exact, lastGuess.Letters.First(l => l.Letter.Index == 3).MatchType);
        Assert.Equal(MatchTypes.Exact, lastGuess.Letters.First(l => l.Letter.Index == 4).MatchType);
    }

    public class MockWordService : IWordService
    {
        public Task<Word> GetRandomWord()
        {
            throw new NotImplementedException();
        }

        public bool IsValid(string word)
        {
            return true;
        }
    }
}
