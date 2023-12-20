using ActvShare.Application.PostManagement.Queries.GetPosts;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Posts;
using ActvShare.Domain.Users.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Application.Test.Common;
using ActvShare.Domain.Users;
using ActvShare.Application.PostManagement.Responses;

namespace ActvShare.Application.Test.PostManagement.Queries;
public class GetPostsQueryHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetPostsQueryHandler _handler;

    public GetPostsQueryHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetPostsQueryHandler(_postRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnPostNotFoundError_When_NoPostsFound()
    {
        // Arrange
        var query = new GetPostsQuery(UserId.CreateUnique().Value);
        
        _postRepositoryMock.Setup(
            x => x.GetAllPostsAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new List<Post> {});

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.Post.PostNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnListOfPostResponses_When_PostsFound()
    {
        // Arrange
        var query = new GetPostsQuery(UserId.CreateUnique().Value);
   
        var content = "content";
        var users = new List<User> { DummyUser.CreateUser("user1"), DummyUser.CreateUser("user2") };
        var posts = new List<Post> { Create_Post(users[0].Id.Value, content), Create_Post(users[1].Id.Value, content) };

        _postRepositoryMock.Setup(
            x => x.GetAllPostsAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(posts);
        
        _userRepositoryMock.SetupSequence(
            x => x.GetUsersByIdsAsync(It.IsAny<List<UserId>>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(users);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<List<PostResponse>>();
        result.Value.Count.Should().Be(posts.Count);
        result.Value.All(postResponse => posts.Any(post => post.Id.Value == postResponse.Id)).Should().BeTrue();
    }

    static Post Create_Post(Guid user, string content){
        var userId = UserId.Create(user);
        return Post.Create(userId, content);
    }
}