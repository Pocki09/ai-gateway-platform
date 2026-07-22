using AiGateway.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;

namespace AiGateway.Infrastructure.Persistence.Repositories;

public sealed class WorkspaceRepository(AppDbContext dbContext) : IWorkspaceRepository
{
    public async Task<Workspace?> GetByIdAsync(
        WorkspaceId id,
        CancellationToken cancellationToken = default)
        => await dbContext.Workspaces
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

    public async Task<Workspace?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
        => await dbContext.Workspaces
            .FirstOrDefaultAsync(w => w.Slug == slug, cancellationToken);

    public async Task<bool> SlugExistsAsync(
        string slug,
        CancellationToken cancellationToken = default)
        => await dbContext.Workspaces
            .AnyAsync(w => w.Slug == slug, cancellationToken);

    public async Task AddAsync(
        Workspace workspace,
        CancellationToken cancellationToken = default)
        => await dbContext.Workspaces.AddAsync(workspace, cancellationToken);

    public void Update(Workspace workspace)
        => dbContext.Workspaces.Update(workspace);
}
