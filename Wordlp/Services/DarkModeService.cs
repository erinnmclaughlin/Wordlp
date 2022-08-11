using Blazored.LocalStorage;

namespace Wordlp.Services;

public class DarkModeService
{
    private const string Key = "darkMode";

    private ILocalStorageService LocalStorage { get; }

    public bool IsEnabled { get; private set; }

    public DarkModeService(ILocalStorageService localStorage)
    {
        LocalStorage = localStorage;
    }

    public async Task InitializeAsync()
    {
        await LocalStorage.RemoveItemAsync("wordlp-DarkMode");

        if (!await LocalStorage.ContainKeyAsync(Key))
            await Enable();
        else
            IsEnabled = await LocalStorage.GetItemAsync<bool>(Key);
    }

    public ValueTask Enable()
    {
        IsEnabled = true;
        return UpdateLocalStorage();
    }

    public ValueTask Disable()
    {
        IsEnabled = false;
        return UpdateLocalStorage();        
    }

    public ValueTask Toggle()
    {
        return IsEnabled ? Disable() : Enable();
    }

    private ValueTask UpdateLocalStorage()
    {
        return LocalStorage.SetItemAsync(Key, IsEnabled);
    }
}