using System.Threading;
using System.Threading.Tasks;
using ActvShare.Application.Authentication.Queries.Login;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.Common.Interfaces.Authentication;
using ActvShare.Domain.Users;
using ActvShare.Application.Authentication.Common;
using ActvShare.Domain.Common.Errors;
using ActvShare.Application.Test.Common;
using ActvShare.Domain.Abstractions;

namespace ActvShare.Application.Test.Authentication.Queries
{
    public class LoginQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private readonly Mock<IRefreshTokenGenerator> _refreshTokenGeneratorMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly LoginQueryHandler _handler;

        public LoginQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            _refreshTokenGeneratorMock = new Mock<IRefreshTokenGenerator>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new LoginQueryHandler(_userRepositoryMock.Object, _jwtTokenGeneratorMock.Object, _refreshTokenGeneratorMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnsAuthenticationResult_When_UserExistsAndCredentialsAreValid()
        {
            // Arrange
            var user = DummyUser.GetDummyUser();

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var query = new LoginQuery("johndoe", "password123");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.user.Name.Should().Be(user.Name);
            result.Value.user.Username.Should().Be(user.Username);
            result.Value.user.Should().BeOfType<AuthenticationResult>();
        }

        [Fact]
        public async Task Handle_Should_ReturnsInvalidCredentialsError_When_UserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(User));

            var query = new LoginQuery("johndoe", "password123");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors[0].Should().Be(Errors.Authentication.InvalidCredentials);
        }
    }
}