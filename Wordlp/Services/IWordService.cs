using Wordlp.Models;

namespace Wordlp.Services;
public interface IWordService
{
    Task<Word> GetRandomWord();
    bool IsValid(string word);
}