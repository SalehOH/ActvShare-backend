using ActvShare.Application.Test.Common;
using ActvShare.Application.UserManagement.Commands.Follow;
using ActvShare.Domain.Common.Errors;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Application.Test.UserManagement.Commands;
public class FollowCommandHandlerTests : Base
{
    private readonly FollowCommandHandler _handler;
    public FollowCommandHandlerTests() : base()
    {
        _handler = new FollowCommandHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnsUserIsAlreadyFollowedError_WhenUserIsAlreadyFollowed()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var userId = UserId.Create(user.Id.Value);

        var followedUser = DummyUser.CreateUser("janedoe");
        var command = new FollowCommand(Username: followedUser.Username, UserId: userId);

        user.FollowUser(UserId.Create(followedUser.Id.Value));

        SetupUserRepositoryMock(user, followedUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Should().Be(Errors.User.UserIsAlreadyFollowed);
    }

    [Fact]
    public async Task Handle_Should_ReturnsFollowResponse_WhenUserIsFollowedSuccessfully()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var userId = UserId.Create(user.Id.Value);

        var followedUser = DummyUser.CreateUser("janedoe");
        var command = new FollowCommand(Username: followedUser.Username, UserId: userId);

        SetupUserRepositoryMock(user, followedUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<FollowResponse>();
        result.Value.Name.Should().Be(followedUser.Name);
        result.Value.Username.Should().Be(followedUser.Username);
    }
}