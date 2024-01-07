using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.Authentication.Commands.Register;
using ActvShare.Application.Authentication.Common;
using ActvShare.Application.Common.Helpers;
using ActvShare.Application.Common.Interfaces.Authentication;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.Common.Responses;
using ActvShare.Application.Test.Common;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Users;
using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace ActvShare.Application.Test.Authentication.Commands
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private readonly Mock<IRefreshTokenGenerator> _refreshTokenGeneratorMock;
        private readonly Mock<ICreateImage> _createImageMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            _refreshTokenGeneratorMock = new Mock<IRefreshTokenGenerator>();
            _createImageMock = new Mock<ICreateImage>();
            
            _handler = new RegisterCommandHandler(
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _jwtTokenGeneratorMock.Object,
                _createImageMock.Object,
                _refreshTokenGeneratorMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnAuthenticationError_When_EmailIsNotUnique()
        {
            // Arrange
            var command = new RegisterCommand("Test User", "tester", "example@test.com", "Test1234!", "Test1234!", PictureMock.GetPicture());

            SetupMock(isEmailUnique: false, isUsernameUnique: true);


            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Should().Be(Errors.User.DuplicateEmail);
        }

        [Fact]
        public async Task Handle_Should_ReturnAuthenticationError_When_UsernameIsNotUnique()
        {
            // Arrange
            var command = new RegisterCommand("Test User", "tester", "example@test.com", "Test1234!", "Test1234!", PictureMock.GetPicture());

            SetupMock(isEmailUnique: true, isUsernameUnique: false);


            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Should().Be(Errors.User.DuplicateUsername);
        }

        [Fact]
        public async Task Handle_Should_ReturnAuthenticationResponse_When_EmailAndUsernameAreUnique()
        {
            // Arrange
            var command = new RegisterCommand("Test User", "tester", "example@test.com", "Test1234!", "Test1234!", PictureMock.GetPicture());

            SetupMock(isEmailUnique: true, isUsernameUnique: true);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.user.Should().BeOfType<AuthenticationResult>();
        }
        [Fact]
        public async Task Handle_Should_CallAddUserAsync_When_EmailAndUsernameAreUnique()
        {
            // Arrange
            var command = new RegisterCommand("Test User", "tester", "example@test.com", "Test1234!", "Test1234!", PictureMock.GetPicture());

            SetupMock(isEmailUnique: true, isUsernameUnique: true);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            _userRepositoryMock.Verify(x =>
                x.AddUserAsync(It.Is<User>(u => u.Username == result.Value.user.Username),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_When_EmailOrUsernameAreNotUnique()
        {
            // Arrange
            var command = new RegisterCommand("Test User", "tester", "example@test.com", "Test1234!", "Test1234!", PictureMock.GetPicture());

            SetupMock(isEmailUnique: false, isUsernameUnique: true);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            _unitOfWorkMock.Verify(x =>
                x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        private void SetupMock(bool isEmailUnique, bool isUsernameUnique)
        {
            _userRepositoryMock.Setup(
                 x => x.IsEmailUniqeAsync(
                     It.IsAny<string>(),
                     It.IsAny<CancellationToken>()))
                 .ReturnsAsync(isEmailUnique);

            _userRepositoryMock.Setup(
                 x => x.IsUsernameUniqeAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(isUsernameUnique);

            _createImageMock.Setup(
                  x => x.Create(
                  It.IsAny<IFormFile>(), CancellationToken.None))
                .ReturnsAsync(new ImageResponse("test", "test", 1));

            _jwtTokenGeneratorMock.Setup(
                x => x.GenerateToken(
                    It.IsAny<User>()))
                .Returns("test");

            _refreshTokenGeneratorMock.Setup(
                x => x.GenerateRefreshToken())
                .Returns("test");
        }
    }
}
