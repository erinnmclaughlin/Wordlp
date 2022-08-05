using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;
using System.Text.Json;
using Wordlp;
using Wordlp.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

using var response = await http.GetAsync(Path.Combine("data", "words.json"));
var words = await response.Content.ReadFromJsonAsync<WordCollection>(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
});

// register services
builder.Services.AddScoped(_ => http);
builder.Services.AddScoped(_ => words ?? new());

await builder.Build().RunAsync();
