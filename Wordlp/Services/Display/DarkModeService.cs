using Blazored.LocalStorage;
using Wordlp.Models.Settings;

namespace Wordlp.Services.Display;

using static LocalStorageSettings;

public class DarkModeService
{
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

    private ILocalStorageService LocalStorage { get; }

    public DarkModeService(ILocalStorageService localStorage)
    {
        LocalStorage = localStorage;
    }

    public async Task InitializeAsync()
    {
        if (!await LocalStorage.ContainKeyAsync(Keys.DarkMode))
            await Enable();
        else
            IsEnabled = await LocalStorage.GetItemAsync<bool>(Keys.DarkMode);
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
        return LocalStorage.SetItemAsync(Keys.DarkMode, IsEnabled);
    }
}