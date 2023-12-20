using ActvShare.Application.PostManagement.Commands.CreateReply;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Posts;
using ActvShare.Domain.Users.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Posts.ValueObjects;
using ActvShare.Application.Test.Common;
using ActvShare.Domain.Users;
using ActvShare.Application.PostManagement.Responses;

namespace ActvShare.Application.Test.PostManagement.Commands;
public class CreateReplyCommandHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateReplyCommandHandler _handler;

    public CreateReplyCommandHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateReplyCommandHandler(_postRepositoryMock.Object, _unitOfWorkMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnPostNotFoundError_When_PostNotFound()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var command = new CreateReplyCommand(PostId.CreateUnique().Value, "Hello", user.Id.Value);

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
    public async Task Handle_Should_ReturnUserNotFoundError_When_UserNotFound()
    {
        // Arrange
        var command = new CreateReplyCommand(PostId.CreateUnique().Value, "Hello", UserId.CreateUnique().Value);
        var post = Post.Create(UserId.CreateUnique(), "Hello");

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
    public async Task Handle_Should_ReturnReplyResponse_When_ReplyCreated()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var userId = UserId.Create(user.Id.Value);

        var post = Post.Create(userId, "Hello");
        
        var command = new CreateReplyCommand(PostId.CreateUnique().Value, post.Content!, user.Id.Value);
        
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
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<ReplyResponse>();
        result.Value.Content.Should().Be(command.Content);
        result.Value.User.Name.Should().Be(user.Name);
        result.Value.User.Username.Should().Be(user.Username);
    }
}