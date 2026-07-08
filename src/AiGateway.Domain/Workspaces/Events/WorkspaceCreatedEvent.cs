using AiGateway.SharedKernel.Domain;

namespace AiGateway.Domain.Workspaces.Events;

public sealed class WorkspaceCreatedEvent(WorkspaceId workspaceId, string name) : DomainEvent
{
    public WorkspaceId WorkspaceId { get; } = workspaceId;
    public string Name { get; } = name;
}