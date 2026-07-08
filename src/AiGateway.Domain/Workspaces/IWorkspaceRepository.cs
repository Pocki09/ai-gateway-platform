namespace AiGateway.Domain.Workspaces;

/// <summary>
/// Repository interface sống ở Domain layer.
/// Implementation sống ở Infrastructure layer.
/// 
/// Điều này đảm bảo Domain không phụ thuộc vào EF Core hay bất kỳ ORM nào.
/// Domain chỉ biết "tôi cần tìm/lưu Workspace theo cách này" — không biết HOW.
/// </summary>
public interface IWorkspaceRepository
{
    Task<Workspace?> GetByIdAsync(WorkspaceId id, CancellationToken cancellationToken = default);
    Task<Workspace?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default);
    Task AddAsync(Workspace workspace, CancellationToken cancellationToken = default);
    void Update(Workspace workspace);
}