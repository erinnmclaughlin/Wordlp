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
        game.Submit();

        var lastGuess = game.Guesses.Last();

        Assert.Equal(LetterMatchType.Exact, lastGuess.Letters.First(l => l.Letter.Index == 0).Result);
        Assert.Equal(LetterMatchType.None, lastGuess.Letters.First(l => l.Letter.Index == 1).Result);
        Assert.Equal(LetterMatchType.None, lastGuess.Letters.First(l => l.Letter.Index == 2).Result);
        Assert.Equal(LetterMatchType.Exact, lastGuess.Letters.First(l => l.Letter.Index == 3).Result);
        Assert.Equal(LetterMatchType.None, lastGuess.Letters.First(l => l.Letter.Index == 4).Result);
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
