namespace Wordlp.Models;

public class WordCollection
{
    public List<Word> Words { get; set; } = new();

    public Word GetRandomWord()
    {
        var randomIndex = new Random().Next(0, Words.Count - 1);
        var word = Words.ElementAt(randomIndex);

        Console.WriteLine(word.Value);

        return word;
    }
}
