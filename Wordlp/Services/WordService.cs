using Wordlp.Models;
using Wordlp.Services.Persistence;

namespace Wordlp.Services;

public class WordService : IWordService
{
    private IGamePersistence GamePersistence { get; }
    private WordCollection WordCollection { get; }

    public WordService(IGamePersistence gamePersistence, WordCollection wordCollection)
    {
        GamePersistence = gamePersistence;
        WordCollection = wordCollection;
    }

    public async Task<Word> GetRandomWord()
    {
        var guessedWords = await GamePersistence.LoadHistory();
        var words = WordCollection.Answers.Where(w => !guessedWords.Any(g => g.Word.Value == w.Value));

        if (words.Any() == false)
            words = WordCollection.Answers;

        var randomIndex = new Random().Next(0, words.Count() - 1);
        return words.ElementAt(randomIndex);
    }

    public bool IsValid(string word)
    {
        return WordCollection.ValidWords.Contains(word, StringComparer.InvariantCultureIgnoreCase);
    }
}
