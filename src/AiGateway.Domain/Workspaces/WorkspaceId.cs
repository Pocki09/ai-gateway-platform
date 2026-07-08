using AiGateway.SharedKernel.Domain;

namespace AiGateway.Domain.Workspaces;

/// <summary>
/// Strong-typed ID cho Workspace.
/// Dùng record để tự động implement equality, hashing và immutability.
/// </summary>
public record WorkspaceId(Guid Value) : EntityId(Value)
{
    public static WorkspaceId New() => new(Guid.NewGuid());
    public static WorkspaceId From(Guid value) => new(value);
}