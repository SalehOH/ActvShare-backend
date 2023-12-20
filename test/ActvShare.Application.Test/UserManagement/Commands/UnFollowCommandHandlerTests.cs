using ActvShare.Application.Test.Common;
using ActvShare.Application.UserManagement.Commands.UnFollow;
using ActvShare.Domain.Common.Errors;
using ActvShare.Application.UserManagement.Responses;

namespace ActvShare.Application.Test.UserManagement.Commands;
public class UnFollowCommandHandlerTests : Base
{
    public UnFollowCommandHandlerTests() : base()
    {
    }

    [Fact]
    public async Task Handle_Should_ReturnsUserIsNotFollowedError_WhenUserIsNotFollowed()
    {
        // Arrange
        var command = new UnFollowCommand (Username: "test", UserId: _userId );
        
        var user = DummyUser.GetDummyUser();
        
        _userRepositoryMock.Setup(
            x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new UnFollowCommandHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Should().Be(Errors.User.UserIsNotFollowed);
    }

    [Fact]
    public async Task Handle_Should_ReturnsFollowResponse_WhenUserIsUnFollowedSuccessfully()
    {
        // Arrange
        var command = new UnFollowCommand (Username: "test", UserId: _userId );
        var user = DummyUser.GetDummyUser();
        user.FollowUser(command.UserId);

        _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var handler = new UnFollowCommandHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<FollowResponse>();
        result.Value.Name.Should().Be(user.Name);
        result.Value.Username.Should().Be(user.Username);
    }
}