using ActvShare.Application.PostManagement.Commands.LikePost;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Posts;
using ActvShare.Domain.Users.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Posts.ValueObjects;
using ActvShare.Domain.Users;
using ActvShare.Application.Test.Common;

namespace ActvShare.Application.Test.PostManagement.Commands;

public class LikePostCommandHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly LikePostCommandHandler _handler;

    public LikePostCommandHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new LikePostCommandHandler(_postRepositoryMock.Object, _unitOfWorkMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnPostNotFoundError_When_PostNotFound()
    {
        // Arrange
        var command = new LikePostCommand(PostId.CreateUnique().Value, UserId.CreateUnique().Value);

        _postRepositoryMock.Setup(
            x => x.GetPostByIdAsync(It.IsAny<PostId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(default (Post));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.Post.PostNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnPostAlreadyLikedError_When_PostAlreadyLiked()
    {
        // Arrange
        var post = Post.Create(UserId.CreateUnique(), "Hello");
        var command = new LikePostCommand(post.Id.Value, UserId.CreateUnique().Value);
        post.AddLike(UserId.Create(command.UserId)); 
        
        _postRepositoryMock.Setup(
            x => x.GetPostByIdAsync(It.IsAny<PostId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(post);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.Post.PostAlreadyLiked);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFoundError_When_UserNotFound()
    {
        // Arrange
        var post = Post.Create(UserId.CreateUnique(), "Hello");
        var command = new LikePostCommand(post.Id.Value, UserId.CreateUnique().Value);
        _postRepositoryMock.Setup(
            x => x.GetPostByIdAsync(It.IsAny<PostId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(post);
        
        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(default (User));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.User.UserNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnTrue_When_PostLikedSuccessfully()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var post = Post.Create(UserId.CreateUnique(), "Hello");
        var command = new LikePostCommand(post.Id.Value, user.Id.Value);

        _postRepositoryMock.Setup(
            x => x.GetPostByIdAsync(It.IsAny<PostId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(post);
        
        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeTrue();
    }
}