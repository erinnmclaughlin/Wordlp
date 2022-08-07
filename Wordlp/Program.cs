using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;
using System.Text.Json;
using Wordlp;
using Wordlp.Models;
using Wordlp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

// register services
var validGuesses = await LoadValidGuesses();
var words = await LoadWords();

builder.Services.AddScoped(_ => http);
builder.Services.AddSingleton(_ => validGuesses);
builder.Services.AddSingleton(_ => words);
builder.Services.AddScoped<DarkModeService>();
builder.Services.AddScoped<GameService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<BrowserResizeService>();

await builder.Build().RunAsync();

async Task<WordCollection> LoadWords()
{
    using var response = await http.GetAsync(Path.Combine("data", "words.json"));
    return await response.Content.ReadFromJsonAsync<WordCollection>(new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    }) ?? new();
}

async Task<ValidWords> LoadValidGuesses()
{
    using var response = await http.GetAsync(Path.Combine("data", "valid.json"));
    return await response.Content.ReadFromJsonAsync<ValidWords>() ?? new();
}