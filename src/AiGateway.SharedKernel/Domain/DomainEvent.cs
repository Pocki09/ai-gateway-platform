namespace AiGateway.SharedKernel.Domain;

/// <summary>
/// Base class cho tất cả domain event.
/// Domain event = "điều gì đó quan trọng vừa xảy ra trong domain".
/// Ví dụ: ApiKeyRevoked, QuotaExceeded, WorkspaceSuspended.
/// </summary>

public abstract class DomainEvent
{
    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; }
    public DateTimeOffset OccurredAt { get; }
}