using AiGateway.SharedKenel.Results;
using MediatR;

namespace AiGateway.Application.Workspaces.Commands.CreateWorkspace;

/// <summary>
/// Command = yêu cầu thay đổi state.
/// Trả về Guid — ID của workspace vừa tạo.
/// </summary>
public sealed record CreateWorkspaceCommand(string Name, string Slug)
    : IRequest<Result<Guid>>;