using AiGateway.SharedKernel.Domain;

namespace AiGateway.SharedKernel.Abstractions;

/// <summary>
/// Aggregate root có khả năng publish domain events.
/// Events được thu thập trong aggregate, sau đó Application layer
/// sẽ dispatch chúng sau khi transaction commit.
/// </summary>
public interface IAggregateRoot
{
    IReadOnlyList<DomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}