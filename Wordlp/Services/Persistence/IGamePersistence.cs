using Wordlp.Models;

namespace Wordlp.Services.Persistence;

public interface IGamePersistence
{
    Task<bool> HasGameHistory();
    Task<SavedGame?> LoadGame();
    Task<List<GameResult>> LoadHistory();
    Task SaveGame(Game game);
    Task Validate();
}
