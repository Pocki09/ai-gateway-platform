using AiGateway.SharedKernel.Domain;
using AiGateway.SharedKernel.Abstractions;

namespace AiGateway.Domain.Common;

/// <summary>
/// Base class cho tất cả aggregate root.
/// Quản lý domain events collection.
/// </summary>
public abstract class AggregateRoot : IAggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = [];

    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}