namespace AiGateway.SharedKernel.Time;

/// <summary>
/// Abstraction cho DateTime — giúp test được mà không phụ thuộc vào system clock.
/// Trong test: inject FakeDateTimeProvider với thời gian cố định.
/// Trong production: inject SystemDateTimeProvider dùng DateTimeOffset.UtcNow.
/// </summary>
public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}