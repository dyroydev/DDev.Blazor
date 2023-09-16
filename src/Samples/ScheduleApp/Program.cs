using ScheduleApp.Services;

// This program writes to the desktop folder.
var localStorageDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\DDev\\ScheduleApp";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDDevBlazor();

builder.Services.AddSingleton<IScheduleService, ScheduleService>();
builder.Services.AddSingleton<IStorage>(sp => new LocalFileSystemStorage(localStorageDirectory));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
