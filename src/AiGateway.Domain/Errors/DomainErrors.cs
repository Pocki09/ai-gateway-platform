using AiGateway.SharedKenel.Results;

namespace AiGateway.Domain.Errors;

/// <summary>
/// Tập trung tất cả domain errors vào một nơi.
/// Tránh hardcode error message ở nhiều chỗ.
/// </summary>
public static class DomainErrors
{
    public static class Workspace
    {
        public static readonly Error NameRequired =
            new("Workspace.NameRequired", "Workspace name is required.");

        public static readonly Error SlugRequired =
            new("Workspace.SlugRequired", "Workspace slug is required.");

        public static readonly Error SlugTooLong =
            new("Workspace.SlugTooLong", "Workspace slug must not exceed 50 characters.");

        public static readonly Error CannotModifyDeleted =
            new("Workspace.CannotModifyDeleted", "Cannot modify a deleted workspace.");

        public static readonly Error AlreadySuspended =
            new("Workspace.AlreadySuspended", "Workspace is already suspended.");

        public static readonly Error AlreadyDeleted =
            new("Workspace.AlreadyDeleted", "Workspace is already deleted.");

        public static readonly Error NotFound =
            new("Workspace.NotFound", "Workspace not found.");

        public static readonly Error SlugAlreadyExists =
            new("Workspace.SlugAlreadyExists", "A workspace with this slug already exists.");
    }
}