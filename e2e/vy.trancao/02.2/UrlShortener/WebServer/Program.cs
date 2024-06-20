var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/_healthz/liveness", () => "OK");

app.MapGet("/api/_healthz/readiness", () => "OK");

app.Run();
