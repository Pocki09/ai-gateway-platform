namespace AiGateway.SharedKenel.Results;

/// <summary>
/// Encapsulates the outcome of an operation — either success or failure.
/// 
/// Tại sao không dùng exception?
/// Exception phù hợp cho unexpected failure (DB crash).
/// Result phù hợp cho expected business failure (quota exceeded, not found).
/// Dùng Result giúp caller phải xử lý failure case, không bị quên.
/// </summary>
public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("Success result cannot have an error.");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("Failure result must have an error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result<T> Success<T>(T value) => new(value, true, Error.None);
    public static Result<T> Failure<T>(Error error) => new(default, false, error);
}

/// <summary>
/// Generic version chứa giá trị khi thành công.
/// </summary>
public class Result<T> : Result
{
    private readonly T? _value;

    internal Result(T? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Chỉ truy cập Value khi IsSuccess = true.
    /// Nếu truy cập khi thất bại → throw ngay thay vì trả null âm thầm.
    /// </summary>
    public T value => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access value of a failure result.");
}