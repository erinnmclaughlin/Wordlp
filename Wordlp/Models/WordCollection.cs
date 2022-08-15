namespace Wordlp.Models;

public class WordCollection
{
    public List<Word> Answers { get; set; } = new();
    public List<string> ValidWords { get; set; } = new();

    public int GetNumber(string value)
    {
        var word = Answers.First(w => w.Value == value);
        return Answers.IndexOf(word) + 1;
    }
    
    public bool IsValid(string word)
    {
        return ValidWords.Contains(word, StringComparer.InvariantCultureIgnoreCase);
    }
}
