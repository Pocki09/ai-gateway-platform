using AiGateway.Application.Abstractions;
using AiGateway.Domain.Workspaces;
using AiGateway.SharedKernel.Abstractions;
using AiGateway.SharedKernel.Domain;
using Microsoft.EntityFrameworkCore;

namespace AiGateway.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Workspace> Workspaces => Set<Workspace>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Bảo EF bỏ qua DomainEvent — đây là domain object, không phải DB entity
        modelBuilder.Ignore<DomainEvent>();

        // Tự động áp dụng tất cả IEntityTypeConfiguration trong assembly này
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var aggregatesWithEvents = ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var aggregate in aggregatesWithEvents)
            aggregate.ClearDomainEvents();

        return result;
    }
}
