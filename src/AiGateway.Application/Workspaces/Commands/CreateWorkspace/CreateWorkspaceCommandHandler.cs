using AiGateway.Application.Abstractions;
using AiGateway.Domain.Errors;
using AiGateway.Domain.Workspaces;
using AiGateway.SharedKenel.Results;
using AiGateway.SharedKernel.Time;
using MediatR;

namespace AiGateway.Application.Workspaces.Commands.CreateWorkspace;

/// <summary>
/// Handler điều phối use case "tạo workspace mới".
/// 
/// Luồng:
/// 1. Kiểm tra slug chưa tồn tại (uniqueness check)
/// 2. Gọi Workspace.Create() để tạo aggregate
/// 3. Lưu vào DB
/// 4. Return ID
/// 
/// Handler không chứa business rule — nó chỉ orchestrate.
/// Business rule nằm trong Workspace.Create().
/// </summary>
public sealed class CreateWorkspaceCommandHandler(
    IWorkspaceRepository repository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateWorkspaceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateWorkspaceCommand request,
        CancellationToken cancellationToken)
    {
        // Bước 1: Kiểm tra slug uniqueness trước khi tạo
        var slugExists = await repository.SlugExistsAsync(request.Slug, cancellationToken);
        if (slugExists)
            return Result.Failure<Guid>(DomainErrors.Workspace.SlugAlreadyExists);

        // Bước 2: Gọi domain factory — domain kiểm tra invariant
        var createResult = Workspace.Create(
            request.Name,
            request.Slug,
            dateTimeProvider.UtcNow
        );

        if (createResult.IsFailure)
            return Result.Failure<Guid>(createResult.Error);

        var workspace = createResult.value;

        // Bước 3: Persist
        await repository.AddAsync(workspace, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(workspace.Id.Value);
    }
}