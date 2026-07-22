namespace AiGateway.Application.Abstractions;

/// <summary>
/// Abstraction cho "lưu tất cả thay đổi trong một transaction".
/// Application layer biết về IUnitOfWork nhưng không biết về DbContext.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}