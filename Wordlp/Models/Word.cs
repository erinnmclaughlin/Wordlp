namespace Wordlp.Models;

public record Word(string Value, string Description)
{
    public List<Letter> GetLetters()
    {
        var letters = new List<Letter>();
        for (int i = 0; i < Value.Length; i++)
        {
            letters.Add(new Letter(Value[i], i));
        }

        return letters;
    }
}

public record Letter(char Value, int Index);
