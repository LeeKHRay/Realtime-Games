using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RealtimeGames.Client;
using RealtimeGames.Client.Services;
using RealtimeGames.Client.Services.GameStates;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IPlayerState, PlayerState>();
builder.Services.AddScoped<IConnect4GameState, Connect4GameState>();
builder.Services.AddScoped<IBattleshipGameState, BattleshipGameState>();

// register the unbound generic type to register each implementation
builder.Services.AddScoped(typeof(IGameHubConnection<>), typeof(GameHubConnection<>));

builder.Services.AddHttpClient("RealtimeGames.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("RealtimeGames.ServerAPI"));

builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
