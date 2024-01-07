using ActvShare.Application.UserManagement.Queries.GetSearchUser;
using ActvShare.Application.Common.Interfaces.Persistance;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Application.UserManagement.Responses;
using System.Collections.Generic;
using ActvShare.Domain.Users;
using ActvShare.Application.Test.Common;

namespace ActvShare.Application.Test.UserManagement.Queries;
public class GetSearchUserQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IChatRepository> _chatRepositoryMock;
    private readonly GetSearchUserQueryHandler _handler;

    public GetSearchUserQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _chatRepositoryMock = new Mock<IChatRepository>();
        _handler = new GetSearchUserQueryHandler(_userRepositoryMock.Object, _chatRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFoundError_When_UserNotFound()
    {
        // Arrange
        var query = new GetSearchUserQuery(Guid.Empty, "test");
        _userRepositoryMock.Setup(
             x => x.GetSearchedUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new List<User>());


        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.User.UserNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnSearchUserResponseList_When_UserFound()
    {
        // Arrange
        var query = new GetSearchUserQuery(Guid.NewGuid(),"john");
        var user = DummyUser.GetDummyUser();

        _userRepositoryMock.Setup(
            x => x.GetSearchedUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new List<User> { user });
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<List<SearchUserResponse>>();
        var searchUserResponses = result.Value;
        searchUserResponses.Should().HaveCount(1);
        var searchUserResponse = searchUserResponses.First();
        searchUserResponse.Name.Should().Be(user.Name);
        searchUserResponse.Username.Should().Be(user.Username);
    }
}