namespace AiGateway.Domain.Workspaces;

/// <summary>
/// Trạng thái của workspace.
/// Dùng enum thay vì string để compiler kiểm tra được exhaustiveness.
/// </summary>
public enum WorkspaceStatus
{
    Active = 1,
    Suspended = 2,
    Deleted = 3
}