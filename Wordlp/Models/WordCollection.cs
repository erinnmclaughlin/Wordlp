namespace Wordlp.Models;

public class WordCollection
{
    public List<Word> Words { get; set; } = new();

    public int GetNumber(string value)
    {
        var word = Words.First(w => w.Value == value);
        return Words.IndexOf(word) + 1;
    }
}
