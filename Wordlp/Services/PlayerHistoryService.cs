using Blazored.LocalStorage;
using Wordlp.Models;

namespace Wordlp.Services;

public class PlayerHistoryService
{
    private const string Key = "playerHistory";
    private ILocalStorageService LocalStorage { get; }

    public PlayerHistoryService(ILocalStorageService localStorage)
    {
        LocalStorage = localStorage;
    }

    public async Task<List<GameResult>> GetHistory()
    {
        await LocalStorage.RemoveItemAsync("wordlp-History");

        return await LocalStorage.GetItemAsync<List<GameResult>>(Key) ?? new();
    }

    public async Task SaveAttempt(Word word, int attemptCount)
    {
        var history = await GetHistory();
        var lastResult = history.FirstOrDefault(h => h.Word.Value == word.Value);

        if (lastResult != null)
            lastResult.AttemptCount = Math.Min(lastResult.AttemptCount, attemptCount);
        else
            history.Add(new GameResult(word, attemptCount));

        await LocalStorage.SetItemAsync(Key, history);
    }
}
