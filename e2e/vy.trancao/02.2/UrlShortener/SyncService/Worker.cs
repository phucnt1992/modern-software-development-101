using Common.Database;
using Common.Entities;
using Cronos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;
using StackExchange.Redis;
using System;

namespace BackupWorker;

public class Worker : BackgroundService
{
    private const string schedule = "0 0 * * *"; // at 12:00 AM
    private readonly CronExpression _cron;
    private readonly ILogger<Worker> _logger;
    private readonly AppDbContext _dbContext;
    private readonly StackExchange.Redis.IDatabase _cache;

    public Worker(ILogger<Worker> logger, AppDbContext dbContext, StackExchange.Redis.IDatabase cache)
    {
        _logger = logger;
        _dbContext = dbContext;
        _cache = cache;
        _cron = CronExpression.Parse(schedule);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var utcNow = DateTime.UtcNow;
            var nextUtc = _cron.GetNextOccurrence(utcNow);
            await Task.Delay(nextUtc.Value - utcNow, stoppingToken);
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await DoBackupAsync();
        }
    }

    private async Task DoBackupAsync()
    {
        const string urlCacheKey = "urls";
        await _dbContext.ShortenedUrls.ForEachAsync(x =>
        {
            var cacheKey = $"{urlCacheKey}:{x.Full}";
            SetUrlCache(cacheKey, x);
        });
    }

    void SetUrlCache(string cacheKey, ShortenedUrl shortenedUrl)
    {

        _cache.StringSet(cacheKey, JsonSerializer.Serialize(shortenedUrl));
        _cache.KeyExpire(cacheKey, TimeSpan.FromSeconds(3600));
    }
}
