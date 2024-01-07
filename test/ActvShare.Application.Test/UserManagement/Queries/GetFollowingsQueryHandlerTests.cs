using ActvShare.Application.UserManagement.Queries.GetFollowings;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Users.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Users;
using ActvShare.Application.Test.Common;

namespace ActvShare.Application.Test.UserManagement.Queries;
public class GetFollowingsQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IChatRepository> _chatRepositoryMock;
    private readonly GetFollowingsQueryHandler _handler;

    public GetFollowingsQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _chatRepositoryMock = new Mock<IChatRepository>();
        _handler = new GetFollowingsQueryHandler(_userRepositoryMock.Object, _chatRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsUserNotFoundError()
    {
        // Arrange
        var query = new GetFollowingsQuery(UserId.CreateUnique().Value);
        
        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(default(User));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Should().Be(Errors.User.UserNotFound);
    }

    [Fact]
    public async Task Handle_UserHasNoFollowings_ReturnsNoFollowersFoundError()
    {
        // Arrange
        var query = new GetFollowingsQuery(UserId.CreateUnique().Value);
        
        var user = DummyUser.GetDummyUser();

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Should().Be(Errors.User.NoFollowersFound);
    }


    [Fact]
    public async Task Handle_UserHasFollowings_ReturnsFollowResponseList()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var query = new GetFollowingsQuery(user.Id.Value);

        var followingUser = DummyUser.CreateUser("janedoe");
        var followingUserId = UserId.Create(followingUser.Id.Value);
        user.FollowUser(followingUserId);

        _userRepositoryMock.SetupSequence(x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(user)
                  .ReturnsAsync(followingUser);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeAssignableTo<IEnumerable<FollowResponse>>();
        var followResponses = result.Value;
        followResponses.Should().HaveCount(1);
        var followResponse = followResponses.First();
        followResponse.Name.Should().Be(followingUser.Name);
        followResponse.Username.Should().Be(followingUser.Username);
    }
}