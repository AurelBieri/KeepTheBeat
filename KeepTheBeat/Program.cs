using KeepTheBeat.Data;
using KeepTheBeat.Database.Services;
using Microsoft.Data.Sqlite;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Register WeatherForecastService as a Singleton
builder.Services.AddSingleton<WeatherForecastService>();


builder.Services.AddBlazoredLocalStorage();

// Register UserService
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<UserService>(serviceProvider => new UserService(connectionString));
builder.Services.AddScoped<SongService>(serviceProvider => new SongService(connectionString));
builder.Services.AddScoped<PlaylistService>(serviceProvider => new PlaylistService(connectionString));

var app = builder.Build();

// Initialize the database
var databaseInitializer = DatabaseInitializer.GetInstance(connectionString);
databaseInitializer.InitializeDatabase();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
