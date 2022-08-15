using Wordlp.Models.Settings;

namespace Wordlp.Models;

public class Guess
{
    private string _value = string.Empty;
    public string Value
    {
        get => _value;
        set
        {
            if (value.Length <= GameSettings.WordLength)
                _value = value;
        }
    }

    public List<Letter> Letters => Value.GetLetters().ToList();
}
