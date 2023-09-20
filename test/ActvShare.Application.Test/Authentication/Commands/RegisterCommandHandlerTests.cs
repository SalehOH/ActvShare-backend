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
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ActvShare.Application.Test.Authentication.Commands
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private readonly Mock<ICreateImage> _createImageMock;

        public RegisterCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            _createImageMock = new Mock<ICreateImage>();
        }

        [Fact]
        public async Task Handle_Should_ReturnAuthenticationError_WhenEmailIsNotUnique()
        {
            // Arrange
            var command = new RegisterCommand("Test User", "tester", "example@test.com", "Test1234!", "Test1234!",  PictureMock.GetPicture());

            SetupMock(isEmailUnique: false, isUsernameUnique: true);

            var handler = new RegisterCommandHandler(
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object, 
                _jwtTokenGeneratorMock.Object,
                _createImageMock.Object);
            
            // Act
            ErrorOr<AuthenticationResult> result = await handler.Handle(command, default);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Should().Be(Errors.User.DuplicateEmail);
        }

        [Fact]
        public async Task Handle_Should_ReturnAuthenticationError_WhenUsernameIsNotUnique()
        {
            // Arrange
            var command = new RegisterCommand("Test User", "tester", "example@test.com", "Test1234!", "Test1234!", PictureMock.GetPicture());

            SetupMock(isEmailUnique: true, isUsernameUnique: false);



            var handler = new RegisterCommandHandler(
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _jwtTokenGeneratorMock.Object,
                _createImageMock.Object);

            // Act
            ErrorOr<AuthenticationResult> result = await handler.Handle(command, default);

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.First().Should().Be(Errors.User.DuplicateUsername);
        }

        [Fact]
        public async Task Handle_Should_ReturnAuthenticationResponse_WhenEmailAndUsernameAreUnique()
        {
            // Arrange
            var command = new RegisterCommand("Test User", "tester", "example@test.com", "Test1234!", "Test1234!", PictureMock.GetPicture());

            var handler = new RegisterCommandHandler(
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _jwtTokenGeneratorMock.Object,
                _createImageMock.Object);
            
            SetupMock(isEmailUnique:true, isUsernameUnique:true);
            
            // Act
            ErrorOr<AuthenticationResult> result = await handler.Handle(command, default);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().BeOfType<AuthenticationResult>();
        }
        [Fact]
        public async Task Handle_Should_CallAddUserAsync_WhenEmailAndUsernameAreUnique()
        {
            // Arrange
            var command = new RegisterCommand("Test User", "tester", "example@test.com", "Test1234!", "Test1234!", PictureMock.GetPicture());

            var handler = new RegisterCommandHandler(
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _jwtTokenGeneratorMock.Object,
                _createImageMock.Object);

            SetupMock(isEmailUnique: true, isUsernameUnique: true);

            // Act
            ErrorOr<AuthenticationResult> result = await handler.Handle(command, default);

            // Assert
            _userRepositoryMock.Verify(x => 
                x.AddUserAsync(It.Is<User>( u => u.Username == result.Value.Username), 
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenEmailOrUsernameAreNotUnique()
        {
            // Arrange
            var command = new RegisterCommand("Test User", "tester", "example@test.com", "Test1234!", "Test1234!", PictureMock.GetPicture());

            var handler = new RegisterCommandHandler(
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _jwtTokenGeneratorMock.Object,
                _createImageMock.Object);

            SetupMock(isEmailUnique: false, isUsernameUnique: true);

            // Act
            ErrorOr<AuthenticationResult> result = await handler.Handle(command, default);

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
        }
    }
}
