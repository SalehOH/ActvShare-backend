using ActvShare.Application.PostManagement.Commands.CreatePost;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Posts;
using ActvShare.Domain.Users.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Application.Common.Helpers;
using ActvShare.Domain.Abstractions;
using ActvShare.Application.Test.Common;
using ActvShare.Domain.Users;
using ActvShare.Application.Common.Responses;
using ActvShare.Application.PostManagement.Responses;
using Microsoft.AspNetCore.Http;

namespace ActvShare.Application.Test.PostManagement.Commands;
public class CreatePostCommandHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICreateImage> _createImageMock;
    private readonly CreatePostCommandHandler _handler;

    public CreatePostCommandHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _createImageMock = new Mock<ICreateImage>();

        _handler = new CreatePostCommandHandler(
            _postRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _createImageMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFoundError_When_UserNotFound()
    {
        // Arrange
        var img = PictureMock.GetPicture();
        var command = new CreatePostCommand(UserId.CreateUnique().Value, "Hello", img);

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(default(User));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.User.UserNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnPostResponse_When_PostCreated()
    {
        // Arrange
        var img = PictureMock.GetPicture();
        var user = DummyUser.GetDummyUser();
        var command = new CreatePostCommand(user.Id.Value, "Hello", img);

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _postRepositoryMock.Setup(
            x => x.AddPostAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _createImageMock.Setup(
            x => x.Create(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ImageResponse(img.FileName, img.FileName, img.Length));
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<PostResponse>();
        result.Value.LikesCount.Should().Be(1);
        result.Value.Content.Should().Be(command.Content);
        result.Value.UserResponse.Should().NotBeNull();
        result.Value.UserResponse.Should().BeOfType<UserResponse>();
        result.Value.UserResponse.Username.Should().Be(user.Username);
        result.Value.ContentPicture.Should().Be(command.ContentPicture?.FileName);
    }
}