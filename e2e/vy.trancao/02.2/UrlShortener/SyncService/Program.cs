using BackupWorker;
using Common.Extensions;
using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDatabase();

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("cache");
builder.Services.AddScoped<IDatabase>(x => redis.GetDatabase());

var host = builder.Build();
host.Run();
