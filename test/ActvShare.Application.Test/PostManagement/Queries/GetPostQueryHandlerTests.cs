using ActvShare.Application.PostManagement.Queries.GetPost;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Posts;
using ActvShare.Domain.Users.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Posts.ValueObjects;
using ActvShare.Domain.Users;
using ActvShare.Application.Test.Common;
using ActvShare.Application.PostManagement.Responses;

namespace ActvShare.Application.Test.PostManagement.Queries;
public class GetPostQueryHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetPostQueryHandler _handler;

    public GetPostQueryHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetPostQueryHandler(_postRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnPostNotFoundError_When_PostNotFound()
    {
        // Arrange
        var query = new GetPostQuery(UserId.CreateUnique().Value);
        
        _postRepositoryMock.Setup(
            x => x.GetPostByIdAsync(It.IsAny<PostId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(default (Post));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.Post.PostNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFoundError_When_UserNotFound()
    {
        // Arrange
        var query = new GetPostQuery(UserId.CreateUnique().Value);

        var post = Post.Create(UserId.CreateUnique(), "content");
        _postRepositoryMock.Setup(
            x => x.GetPostByIdAsync(It.IsAny<PostId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(post);
        
        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(default (User));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.User.UserNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnPostWithRepliesResponse_When_PostAndUserFound()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var userId = UserId.Create(user.Id.Value);

        var post = Post.Create(userId, "content");
        var replyContent = "reply content";

        post.AddReply(replyContent, UserId.CreateUnique());
        post.AddReply(replyContent + 2, UserId.CreateUnique());

        var query = new GetPostQuery(post.Id.Value);
        
        _postRepositoryMock.Setup(
            x => x.GetPostByIdAsync(It.IsAny<PostId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(post);
        
        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<PostWithRepliesResponse>();
        result.Value.Post.Should().NotBeNull();
        result.Value.Post.Should().BeOfType<PostResponse>();
        result.Value.Post.Id.Should().Be(post.Id.Value);
        result.Value.Post.Content.Should().Be(post.Content);
        result.Value.Replies.Should().NotBeNull();
        result.Value.Replies.Should().BeOfType<List<ReplyResponse>>();
        result.Value.Replies.Should().HaveCount(2);
        result.Value.Replies.First().Content.Should().Be(replyContent);
    }
}