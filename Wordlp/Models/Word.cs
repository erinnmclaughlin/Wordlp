namespace Wordlp.Models;

public record Word
{
    public string Value { get; }
    public string Description { get; }
    public List<Letter> Letters { get; }

    public Word(string value, string description)
    {
        Value = value;
        Description = description;
        Letters = value.GetLetters().ToList();
    }
}
