using ActvShare.Application.PostManagement.Commands.RemoveLike;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Posts;
using ActvShare.Domain.Users.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Posts.ValueObjects;
using ActvShare.Application.Test.Common;

public class RemoveLikeCommandHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RemoveLikeCommandHandler _handler;

    public RemoveLikeCommandHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RemoveLikeCommandHandler(_postRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnPostNotFoundError_When_PostNotFound()
    {
        // Arrange
        var command = new RemoveLikeCommand(UserId.CreateUnique().Value, PostId.CreateUnique().Value);
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
    public async Task Handle_Should_ReturnPostNotLikedError_When_PostNotLiked()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var post = Post.Create(UserId.CreateUnique(), "Hello");
        var command = new RemoveLikeCommand(post.Id.Value, user.Id.Value);
    
        
        _postRepositoryMock.Setup(
            x => x.GetPostByIdAsync(It.IsAny<PostId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(post);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.Post.PostNotLiked);
    }

    [Fact]
    public async Task Handle_Should_ReturnTrue_When_LikeRemovedSuccessfully()
    {
        // Arrange
        var command = new RemoveLikeCommand(UserId.CreateUnique().Value, PostId.CreateUnique().Value);
        var post = Post.Create(UserId.CreateUnique(), "Hello");

        post.AddLike(UserId.Create(command.UserId));
        _postRepositoryMock.Setup(x => x.GetPostByIdAsync(It.IsAny<PostId>(), It.IsAny<CancellationToken>())).ReturnsAsync(post);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeTrue();
    }
}