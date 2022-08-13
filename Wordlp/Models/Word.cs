namespace Wordlp.Models;

public record Word(string Value, string Description)
{
    public bool Contains(char letter) => Value.Contains(letter);
    public bool ContainsAtIndex(char letter, int index) => Value.ElementAtOrDefault(index) == letter;

}
