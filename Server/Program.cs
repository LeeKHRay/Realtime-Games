using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using RealtimeGames.Server.Areas.Identity.Models;
using RealtimeGames.Server.Data;
using RealtimeGames.Server.Filters;
using RealtimeGames.Server.Hubs;
using RealtimeGames.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("RealtimeGamesDb") ?? throw new InvalidOperationException("Connection string 'RealtimeGamesDb' not found.");
builder.Services.AddDbContext<RealtimeGamesDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<RealtimeGamesDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, RealtimeGamesDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

builder.Services.AddSingleton<GameHubPlayersStorage>();

builder.Services.AddScoped<RedirectAuthenticatedUserFilter>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // create database and tables
    var dbContext = scope.ServiceProvider.GetRequiredService<RealtimeGamesDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseResponseCompression();
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.MapHub<Connect4Hub>("/hubs/connect4");
app.MapHub<BattleshipHub>("/hubs/battleship");

app.MapFallbackToFile("index.html");

app.Run();
