using ActvShare.Application.Test.Common;
using ActvShare.Application.UserManagement.Commands.UnFollow;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Application.Test.UserManagement.Commands;
public class UnFollowCommandHandlerTests : Base
{
    private readonly UnFollowCommandHandler _handler;
    public UnFollowCommandHandlerTests() : base()
    {
        _handler = new UnFollowCommandHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnsUserIsNotFollowedError_WhenUserIsNotFollowed()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var unfollowedUser = DummyUser.CreateUser("janedoe");

        var userId = UserId.Create(user.Id.Value);
        var command = new UnFollowCommand(Username: unfollowedUser.Username, UserId: userId);

        SetupUserRepositoryMock(user, unfollowedUser);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Should().Be(Errors.User.UserIsNotFollowed);
    }

    [Fact]
    public async Task Handle_Should_ReturnsFollowResponse_WhenUserIsUnFollowedSuccessfully()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var unfollowedUser = DummyUser.CreateUser("janedoe");

        var userId = UserId.Create(user.Id.Value);
        var command = new UnFollowCommand(Username: unfollowedUser.Username, UserId: userId);
        user.FollowUser(UserId.Create(unfollowedUser.Id.Value));

        SetupUserRepositoryMock(user, unfollowedUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeTrue();
    }
}