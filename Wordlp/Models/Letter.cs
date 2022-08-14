namespace Wordlp.Models;

public record Letter(int Index, char Value);

public static class StringExtensions
{
    public static IEnumerable<Letter> GetLetters(this string value)
    {
        for (int i = 0; i < value.Length; i++)
            yield return new Letter(i, value[i]);
    }
}