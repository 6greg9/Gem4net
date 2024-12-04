using DemoEqpApp.Components;
using DemoEqpApp.Services;
using Gem4Net;
using Gem4NetRepository;
using Gem4NetRepository.Model;
using Secs4Net;
using Secs4Net.Sml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOptions();

builder.Services.Configure<GemEqpAppOptions>(
             builder.Configuration.GetSection("GemEqpAppOptions"));
builder.Services.Configure<SecsGemOptions>(
     builder.Configuration.GetSection("secs4net"));

builder.Services.AddDbContext<GemDbContext>();
builder.Services.AddSingleton<GemRepository>();
builder.Services.AddSingleton<GemEqpService>();
builder.Services.AddSingleton<ISecsGemLogger, SecsLogger>();

builder.Services.AddSingleton<SecsGemManager>();

builder.Services.AddSingleton<MyBackgroundWorker>();

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

// ¥ý±Ò°Ê
var host = app.RunAsync();

var services = builder.Services.BuildServiceProvider();
var backgroundWorker = services.GetRequiredService<MyBackgroundWorker>() as MyBackgroundWorker;

await host;
