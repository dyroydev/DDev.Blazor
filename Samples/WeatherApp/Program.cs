using WeatherApp.Components;
using WeatherApp.Services;
using DDev.Blazor.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add required http clients
builder.Services.AddHttpClient("geonorge", client => client.BaseAddress = new Uri("https://ws.geonorge.no"));
builder.Services.AddHttpClient("open-meteo", client => client.BaseAddress = new Uri("https://api.open-meteo.com/"));

// Add services
builder.Services.AddSingleton<IWeatherClient, OpenMeteoWeatherClient>();
builder.Services.AddSingleton<ILocationClient, NorwegianLocationClient>();
builder.Services.AddScoped<FavoriteLocationsService>();

// Add DDev Blazor
builder.Services.AddDDevBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
