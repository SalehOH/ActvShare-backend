using ActvShare.Application.UserManagement.Queries.GetUser;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Users;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Application.Test.Common;

namespace ActvShare.Application.Test.UserManagement.Queries;
public class GetUserQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetUserQueryHandler _handler;

    public GetUserQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetUserQueryHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFoundError_When_UserNotFound()
    {
        // Arrange
        var query = new GetUserQuery("test");
        _userRepositoryMock.Setup(
            x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(default (User));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.User.UserNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnUser_When_UserFound()
    {
        // Arrange
        var query = new GetUserQuery("janedoe");
        var user = DummyUser.CreateUser("janedoe");

        _userRepositoryMock.Setup(
            x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<User>();
        result.Value.Username.Should().Be(user.Username);
        result.Value.Name.Should().Be(user.Name);
    }
}