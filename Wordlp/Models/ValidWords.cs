namespace Wordlp.Models;

public class ValidWords : List<string>
{
    public bool IsValid(string word)
    {
        return this.Contains(word, StringComparer.InvariantCultureIgnoreCase);
    }
}
