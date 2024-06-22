using Common.Database;
using Common.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("APP_DB_URL");
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

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

    app.MapPost("/api/urls", (UrlShorteningRequest request) =>
    {
        var shortenedUrl = dbContext.ShortenedUrls.FirstOrDefault(x => x.Url == request.Url);
        if (shortenedUrl is null)
        {
            return Results.BadRequest();
        }

        return Results.Redirect(shortenedUrl.Full);
    });

    app.Run();
}
