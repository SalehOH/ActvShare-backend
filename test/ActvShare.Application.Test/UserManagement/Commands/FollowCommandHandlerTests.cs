using ActvShare.Application.Test.Common;
using ActvShare.Application.UserManagement.Commands.Follow;
using ActvShare.Domain.Common.Errors;
using ActvShare.Application.UserManagement.Responses;

namespace ActvShare.Application.Test.UserManagement.Commands;
public class FollowCommandHandlerTests : Base
{
    public FollowCommandHandlerTests() : base()
    {
    }

    [Fact]
    public async Task Handle_Should_ReturnsUserIsAlreadyFollowedError_WhenUserIsAlreadyFollowed()
    {
        // Arrange
        var command = new FollowCommand (Username: "test", UserId: _userId );
        
        var user = DummyUser.GetDummyUser();

        user.FollowUser(command.UserId); // User is already followed
        
        _userRepositoryMock.Setup(
            x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new FollowCommandHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Should().Be(Errors.User.UserIsAlreadyFollowed);
    }

    [Fact]
    public async Task Handle_Should_ReturnsFollowResponse_WhenUserIsFollowedSuccessfully()
    {
        // Arrange
        var command = new FollowCommand (Username: "test", UserId: _userId );
        var user = DummyUser.GetDummyUser();

        _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var handler = new FollowCommandHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object);
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<FollowResponse>();
        result.Value.Name.Should().Be(user.Name);
        result.Value.Username.Should().Be(user.Username);
    }
}