using System.Threading;
using System.Threading.Tasks;
using ActvShare.Application.Authentication.Queries.Login;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.Common.Interfaces.Authentication;
using ActvShare.Domain.Users;
using ActvShare.Application.Authentication.Common;
using ActvShare.Domain.Common.Errors;
using ActvShare.Application.Test.Common;

namespace ActvShare.Application.Test.Authentication.Queries
{
    public class LoginQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;

        public LoginQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        }
        
        [Fact]
        public async Task Handle_Should_ReturnsAuthenticationResult_When_UserExistsAndCredentialsAreValid()
        {
            // Arrange
            var user = DummyUser.GetDummyUser();

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var handler = new LoginQueryHandler(_userRepositoryMock.Object, _jwtTokenGeneratorMock.Object);
            var query = new LoginQuery ("johndoe", "password123");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Name.Should().Be(user.Name);
            result.Value.Username.Should().Be(user.Username);
            result.Value.Should().BeOfType<AuthenticationResult>();
        }

        [Fact]
        public async Task Handle_Should_ReturnsInvalidCredentialsError_When_UserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(User));

            var handler = new LoginQueryHandler(_userRepositoryMock.Object, _jwtTokenGeneratorMock.Object);
            var query = new LoginQuery ("johndoe", "password123");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Should().Be(Errors.Authentication.InvalidCredentials);
        }
    }
}