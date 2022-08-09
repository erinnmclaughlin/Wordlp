using System.Text.Json;
using Wordlp.Models;

namespace Wordlp.Tests;

public class WordsTests
{
    [Fact]
    public void ValidWordListContainsAllWords()
    {
        var words = File.ReadAllText("../../../../../Wordlp/wwwroot/data/words.json");
        var wordCollection = JsonSerializer.Deserialize<WordCollection>(words, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        Assert.NotEmpty(wordCollection.Words);

        var valid = File.ReadAllText("../../../../../Wordlp/wwwroot/data/valid.json");
        var validWords = JsonSerializer.Deserialize<List<string>>(valid)!;
        Assert.NotEmpty(validWords);

        foreach (var word in wordCollection.Words)
            Assert.True(validWords.Contains(word.Value, StringComparer.InvariantCultureIgnoreCase));
    }
}