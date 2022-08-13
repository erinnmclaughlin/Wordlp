using Blazored.LocalStorage;
using Wordlp.Models;

using static Wordlp.Shared.Settings.LocalStorageSettings;

namespace Wordlp.Services.Persistence;

public class LocalStoragePersistence : IGamePersistence
{
    private ILocalStorageService LocalStorage { get; }

	public LocalStoragePersistence(ILocalStorageService localStorage)
	{
		LocalStorage = localStorage;
	}

	public async Task<SavedGame?> LoadGame()
	{
		return await LocalStorage.GetItemAsync<SavedGame>(Keys.SavedGame);
    }

	public async Task<List<GameResult>> LoadHistory()
	{
        return await LocalStorage.GetItemAsync<List<GameResult>>(Keys.PlayerHistory) ?? new();
    }

	public async Task SaveGame(Game game)
	{
		var savedGame = new SavedGame(game.Guesses, game.Solution);
        await LocalStorage.SetItemAsync(Keys.SavedGame, savedGame);

		if (game.IsGameOver)
			await AddToHistory(game);
    }

	private async Task AddToHistory(Game game)
	{
        var history = await LoadHistory();
        var lastResult = history.FirstOrDefault(h => h.Word.Value == game.Solution.Value);

        if (lastResult == null)
			history.Add(new GameResult(game.Solution, game.Guesses.Count, game.IsWin));

        await LocalStorage.SetItemAsync(Keys.PlayerHistory, history);
    }
}
