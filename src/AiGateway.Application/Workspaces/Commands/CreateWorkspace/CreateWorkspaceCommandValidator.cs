using FluentValidation;

namespace AiGateway.Application.Workspaces.Commands.CreateWorkspace;

/// <summary>
/// Validation ở Application boundary — kiểm tra format/structure của request.
/// Khác với Domain validation — Domain kiểm tra business invariant.
/// 
/// Ví dụ:
/// - Application: "Slug phải match regex ^[a-z0-9-]+$" (format rule)
/// - Domain: "Slug không được rỗng" (invariant)
/// </summary>
public sealed class CreateWorkspaceCommandValidator : AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(50).WithMessage("Slug must not exceed 50 characters.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug must contain only lowercase letters, numbers, and hyphens.");
    }
}