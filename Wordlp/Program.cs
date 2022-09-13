using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;
using System.Text.Json;
using Wordlp;
using Wordlp.Models;
using Wordlp.Services;
using Wordlp.Services.Display;
using Wordlp.Services.Persistence;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

// register services
var words = await LoadWords();

builder.Services.AddScoped(_ => http);
builder.Services.AddSingleton(_ => words);
builder.Services.AddScoped<BrowserResizeService>();
builder.Services.AddScoped<DarkModeService>();
builder.Services.AddScoped<Game>();
builder.Services.AddScoped<IGamePersistence, LocalStoragePersistence>();
builder.Services.AddScoped<IWordService, WordService>();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();

async Task<WordCollection> LoadWords()
{
    var jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    var answers = await http.GetAsync(Path.Combine("data", "words_v3.json"));
    var validWords = await http.GetAsync(Path.Combine("data", "valid.json"));

    return new WordCollection
    {
        Answers = await answers.Content.ReadFromJsonAsync<List<Word>>(jsonOptions) ?? new(),
        ValidWords = await validWords.Content.ReadFromJsonAsync<List<string>>(jsonOptions) ?? new()
    };
}
