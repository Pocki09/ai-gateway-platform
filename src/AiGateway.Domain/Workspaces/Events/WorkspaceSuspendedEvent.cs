using AiGateway.SharedKernel.Domain;

namespace AiGateway.Domain.Workspaces.Events;

public sealed class WorkspaceSuspendedEvent(WorkspaceId workspaceId, string reason) : DomainEvent
{
    public WorkspaceId WorkspaceId { get; } = workspaceId;
    public string Reason { get; } = reason;
}