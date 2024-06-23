using Common.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Common.Database;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
}
