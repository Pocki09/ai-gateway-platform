using AiGateway.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AiGateway.Infrastructure;

/// <summary>
/// Factory này chỉ dùng cho EF Core CLI tools (migrations).
/// Không ảnh hưởng đến runtime — chỉ chạy khi gọi dotnet ef.
/// </summary>
public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=aigateway_dev;Username=aigateway;Password=aigateway_dev_password",
            npgsqlOptions => npgsqlOptions
                .MigrationsHistoryTable("__ef_migrations_history")
                .CommandTimeout(30));

        return new AppDbContext(optionsBuilder.Options);
    }
}
