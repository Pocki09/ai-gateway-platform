using AiGateway.Domain.Workspaces;
using FluentAssertions;

namespace AiGateway.Domain.Tests.Workspaces;

public class WorkspaceTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.UtcNow;

    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        var result = Workspace.Create("Team Alpha", "team-alpha", Now);

        result.IsSuccess.Should().BeTrue();
        result.value.Name.Should().Be("Team Alpha");
        result.value.Slug.Should().Be("team-alpha");
        result.value.Status.Should().Be(WorkspaceStatus.Active);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldFail()
    {
        var result = Workspace.Create("", "team-alpha", Now);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Workspace.NameRequired");
    }

    [Fact]
    public void Suspend_ActiveWorkspace_ShouldSucceed()
    {
        var workspace = Workspace.Create("Team Alpha", "team-alpha", Now).value;

        var result = workspace.Suspend("Abuse detected", Now);

        result.IsSuccess.Should().BeTrue();
        workspace.Status.Should().Be(WorkspaceStatus.Suspended);
    }

    [Fact]
    public void Activate_DeletedWorkspace_ShouldFail()
    {
        var workspace = Workspace.Create("Team Alpha", "team-alpha", Now).value;
        workspace.Delete(Now);

        var result = workspace.Activate(Now);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Workspace.CannotModifyDeleted");
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent()
    {
        var result = Workspace.Create("Team Alpha", "team-alpha", Now);

        result.value.DomainEvents.Should().HaveCount(1);
        result.value.DomainEvents[0].Should().BeOfType<AiGateway.Domain.Workspaces.Events.WorkspaceCreatedEvent>();
    }
}