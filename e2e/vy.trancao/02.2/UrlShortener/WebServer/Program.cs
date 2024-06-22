using Common.Database;
using Common.Entities;
using Common.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Refit;
using StackExchange.Redis;
using System;
using System.Reflection;
using System.Text.Json;
using WebServer.Clients;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var connectionString = Environment.GetEnvironmentVariable("APP_DB_URL");
//var connectionString = "Username=postgres;Password=121002;Host=localhost;Database=ShortenUrl";
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(connectionString,
        b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
});

builder.Services
    .AddRefitClient<IUlvisApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://ulvis.net/API"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    const string URL_CACHE_KEY = "urls";

    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var ulvisApi = scope.ServiceProvider.GetRequiredService<IUlvisApi>();
    ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("cache");
    IDatabase cache = redis.GetDatabase();
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }

    app.MapGet("/api/_healthz/liveness", () =>
    {
        return string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APP_DB_URL"))
            ? Results.BadRequest("Error")
            : Results.Ok("Ok");
    });

    app.MapGet("/api/_healthz/readiness", () =>
    {
        try
        {
            var anyUrls = dbContext.ShortenedUrls.Any();
            return Results.Ok("Ok");
        }
        catch
        {
            return Results.BadRequest("Error");
        }
    });

    app.MapPost("/api/urls", async ([FromBody] UrlShorteningRequest request) =>
    {
        HttpClient client = new HttpClient();
        var shortenedUrl = GetShortenedUrl(request.Url);
        if (shortenedUrl is null)
        {
            var shorteningResult = await ulvisApi.ConvertUrl(request.Url);
            if (!shorteningResult.Success)
            {
                return Results.BadRequest();
            }

            shortenedUrl = new ShortenedUrl
            {
                Url = shorteningResult.Data?.Url,
                Full = shorteningResult.Data?.Full,
                Content = await client.GetStringAsync(request.Url)
            };

            AddShortenedUrl(request.Url, shortenedUrl);
        }

        return Results.Ok(shortenedUrl.Url);
    });

    ShortenedUrl? GetShortenedUrl(string url)
    {
        var cacheKey = $"{URL_CACHE_KEY}:{url}";
        var cachedUrl = cache.StringGet($"{URL_CACHE_KEY}:{url}");
        Console.WriteLine("Get from key: " + cacheKey);
        if (!string.IsNullOrEmpty(cachedUrl))
        {
            Console.WriteLine("Cache hit");
            return JsonSerializer.Deserialize<ShortenedUrl>(cachedUrl);
        }
        else
        {
            Console.WriteLine("Cache miss");
            return dbContext.ShortenedUrls.FirstOrDefault(x => x.Full == url);
        }
    }

    void AddShortenedUrl(string fullUrl, ShortenedUrl shortenedUrl)
    {
        var cacheKey = $"{URL_CACHE_KEY}:{fullUrl}";
        dbContext.ShortenedUrls.Add(shortenedUrl);
        dbContext.SaveChanges();
        Console.WriteLine("Write to cache with key: " + cacheKey);
        cache.StringSet(cacheKey, JsonSerializer.Serialize(shortenedUrl));
        cache.KeyExpire(cacheKey, TimeSpan.FromSeconds(3600));
    }

    app.MapGet("/api/urls", () =>
    {
        return Results.Ok(dbContext.ShortenedUrls.ToArray());
    });

    app.Run();
}



