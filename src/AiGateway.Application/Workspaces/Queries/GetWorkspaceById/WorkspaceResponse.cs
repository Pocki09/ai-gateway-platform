namespace AiGateway.Application.Workspaces.Queries.GetWorkspaceById;

/// <summary>
/// DTO trả về cho client — KHÔNG expose domain entity trực tiếp.
/// 
/// Tại sao không trả Workspace entity thẳng ra API?
/// 1. Domain entity có thể thay đổi cấu trúc → vô tình break API contract
/// 2. Có thể expose field không muốn client thấy
/// 3. Khó versioning nếu dùng entity trực tiếp
/// </summary>
public sealed record WorkspaceResponse(
    Guid Id,
    string Name,
    string Slug,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);