namespace AiGateway.Api.Common;

/// <summary>
/// Schema chuẩn cho tất cả API response.
/// Client luôn nhận được format nhất quán — dễ tích hợp.
/// </summary>
public sealed record ApiResponse<T>(
    bool Success,
    T? Data,
    ApiError? Error
);

public sealed record ApiError(
    string Code, 
    string Message, 
    IEnumerable<string>? Details = null
);