using AiGateway.Domain.Common;
using AiGateway.Domain.Errors;
using AiGateway.Domain.Workspaces.Events;
using AiGateway.SharedKenel.Results;

namespace AiGateway.Domain.Workspaces;

/// <summary>
/// Workspace là tenant boundary chính của hệ thống.
/// 
/// Invariants (bất biến) cần bảo vệ:
/// 1. Tên workspace không được rỗng
/// 2. Slug phải unique (kiểm tra ở application layer)
/// 3. Workspace Deleted không thể Active trở lại
/// 4. Workspace Suspended không thể gửi AI request
/// 
/// Tại sao dùng private set và constructor private?
/// → Buộc tạo Workspace phải qua factory method Create()
/// → Đảm bảo invariant được kiểm tra khi tạo
/// </summary>
public sealed class Workspace : AggregateRoot
{
    // Private constructor → chỉ tạo được qua Create()
    private Workspace() { }

    public WorkspaceId Id { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    
    /// <summary>
    /// Slug dùng làm identifier thân thiện, ví dụ: "team-alpha"
    /// Phải unique trong hệ thống.
    /// </summary>
    public string Slug { get; private set; } = null!;
    
    public WorkspaceStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    /// <summary>
    /// Factory method — điểm vào duy nhất để tạo Workspace hợp lệ.
    /// Return Result thay vì throw exception để caller phải xử lý failure.
    /// </summary>
    public static Result<Workspace> Create(string name, string slug, DateTimeOffset now)
    {
        // Validate invariant trước khi tạo
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Workspace>(DomainErrors.Workspace.NameRequired);

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure<Workspace>(DomainErrors.Workspace.SlugRequired);

        if (slug.Length > 50)
            return Result.Failure<Workspace>(DomainErrors.Workspace.SlugTooLong);

        var workspace = new Workspace
        {
            Id = WorkspaceId.New(),
            Name = name.Trim(),
            Slug = slug.Trim().ToLowerInvariant(),
            Status = WorkspaceStatus.Active,
            CreatedAt = now,
            UpdatedAt = now
        };

        // Phát domain event để các subscriber biết workspace được tạo
        workspace.RaiseDomainEvent(new WorkspaceCreatedEvent(workspace.Id, workspace.Name));

        return Result.Success(workspace);
    }

    /// <summary>
    /// Suspend workspace — workspace bị suspend không thể gửi AI request.
    /// </summary>
    public Result Suspend(string reason, DateTimeOffset now)
    {
        if (Status == WorkspaceStatus.Deleted)
            return Result.Failure(DomainErrors.Workspace.CannotModifyDeleted);

        if (Status == WorkspaceStatus.Suspended)
            return Result.Failure(DomainErrors.Workspace.AlreadySuspended);

        Status = WorkspaceStatus.Suspended;
        UpdatedAt = now;

        RaiseDomainEvent(new WorkspaceSuspendedEvent(Id, reason));

        return Result.Success();
    }

    /// <summary>
    /// Reactivate workspace từ Suspended → Active.
    /// Không thể từ Deleted → Active.
    /// </summary>
    public Result Activate(DateTimeOffset now)
    {
        if (Status == WorkspaceStatus.Deleted)
            return Result.Failure(DomainErrors.Workspace.CannotModifyDeleted);

        Status = WorkspaceStatus.Active;
        UpdatedAt = now;

        return Result.Success();
    }

    /// <summary>
    /// Soft delete — không xóa thật vì cần giữ audit trail.
    /// </summary>
    public Result Delete(DateTimeOffset now)
    {
        if (Status == WorkspaceStatus.Deleted)
            return Result.Failure(DomainErrors.Workspace.AlreadyDeleted);

        Status = WorkspaceStatus.Deleted;
        UpdatedAt = now;

        return Result.Success();
    }

    /// <summary>
    /// Business rule: workspace này có thể nhận AI request không?
    /// </summary>
    public bool CanAcceptRequests() => Status == WorkspaceStatus.Active;
}