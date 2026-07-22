using AiGateway.Application.Abstractions;
using AiGateway.Domain.Workspaces;
using AiGateway.Infrastructure.Persistence;
using AiGateway.Infrastructure.Persistence.Repositories;
using AiGateway.Infrastructure.Time;
using AiGateway.SharedKernel.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiGateway.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions => npgsqlOptions
                    .MigrationsHistoryTable("__ef_migrations_history")
                    .CommandTimeout(30)));

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<AppDbContext>());

        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();

        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}
