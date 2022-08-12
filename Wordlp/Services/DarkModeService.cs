using Blazored.LocalStorage;

namespace Wordlp.Services;

public class DarkModeService
{
    private const string Key = "darkMode";

    private ILocalStorageService LocalStorage { get; }

    private bool _isEnabled;
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (_isEnabled == value) return;

            _isEnabled = value;
            OnToggle?.Invoke(this, value);
        }
    }

    public event EventHandler<bool>? OnToggle;

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