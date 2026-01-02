using AI_Video_Stack.Server.Services;

using AI_Video_Stack.Server.Services.Contracts;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Options
builder.Services.Configure<OllamaOptions>(builder.Configuration.GetSection("Ollama"));

builder.Services.Configure<TtsServiceOptions>(builder.Configuration.GetSection("TtsService")); 
builder.Services.Configure<ShotstackOptions>(builder.Configuration.GetSection("Shotstack"));
builder.Services.Configure<StaticAssetsOptions>(builder.Configuration.GetSection("StaticAssets")); 
// Http Clients
builder.Services.AddHttpClient("Ollama");
builder.Services.AddHttpClient("TtsService"); 
builder.Services.AddHttpClient("Shotstack");
builder.Services.AddHttpClient("Ollama", client =>
{
    client.Timeout = TimeSpan.FromMinutes(5);
});
builder.Services.AddHttpClient("TtsService", client =>
{
    client.Timeout = TimeSpan.FromMinutes(5);
});
// Services
// Interface registrations
builder.Services.AddScoped<IOllamaService, OllamaService>();
builder.Services.AddScoped<ITtsService, TtsService>();
builder.Services.AddScoped<IShotstackService, ShotstackService>(); 
// CORS for React
builder.Services.AddCors(o => o.AddPolicy("Frontend", p => p.WithOrigins("http://localhost:5173") .AllowAnyHeader() .AllowAnyMethod()));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy => { policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod().AllowCredentials(); });
});
var app = builder.Build();
app.UseCors("AllowReactApp");
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
// Options
public record OllamaOptions { public string BaseUrl { get; init; } = ""; public string Model { get; init; } = ""; }
public record TtsServiceOptions { public string BaseUrl { get; init; } = ""; }
public record ShotstackOptions { public string BaseUrl { get; init; } = ""; public bool Stage { get; init; } = true; public string ApiKey { get; init; } = ""; }
public record StaticAssetsOptions { public string PublicOrigin { get; init; } = ""; public string AssetsPath { get; init; } = ""; }