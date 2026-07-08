namespace AiGateway.SharedKenel.Results;

/// <summary>
/// Đại diện cho một lỗi nghiệp vụ — bất biến, so sánh bằng giá trị.
/// Dùng record để tự động implement equality.
/// </summary>
public record Error(string Code, string Message)
{
    
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NotFound = new("General.NotFound", "Resource not found");
    public static readonly Error Unauthorized = new("General.Unauthorized", "Unauthorized");
    public static readonly Error Forbidden = new("General.Forbidden", "Forbidden");

    public static Error validation(string message) => new("General.Validation", message);
}