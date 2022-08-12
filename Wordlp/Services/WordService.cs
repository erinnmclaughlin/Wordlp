using Wordlp.Models;

namespace Wordlp.Services;

public class WordService
{
    private PlayerHistoryService PlayerHistory { get; }
    private WordCollection WordCollection { get; }

    public WordService(PlayerHistoryService playerHistory, WordCollection wordCollection)
    {
        PlayerHistory = playerHistory;
        WordCollection = wordCollection;
    }

    public async Task<Word> GetRandomWord()
    {
        var guessedWords = await PlayerHistory.GetHistory();
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
