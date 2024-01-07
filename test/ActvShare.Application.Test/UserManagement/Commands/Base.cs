using ActvShare.Application.UserManagement.Commands.Follow;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Users;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Application.Test.UserManagement.Commands;
public class Base
{
    protected readonly Mock<IUserRepository> _userRepositoryMock;
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public Base()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task Handle_Should_ReturnsUserNotFoundError_WhenTheUserDoesNotExists()
    {
        // Arrange
        var command = new FollowCommand(Username: "test", UserId.CreateUnique());
        _userRepositoryMock.Setup(
            x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(User));

        var handler = new FollowCommandHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Should().Be(Errors.User.UserNotFound);
    }
    protected void SetupUserRepositoryMock(User user, User otherUser)
    {
        _userRepositoryMock.Setup(
            x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(otherUser);

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
    }
}