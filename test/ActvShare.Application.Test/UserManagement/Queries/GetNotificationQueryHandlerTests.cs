using ActvShare.Application.UserManagement.Queries.GetNotifications;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Users.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Application.UserManagement.Responses;
using System.Collections.Generic;
using ActvShare.Domain.Users;
using ActvShare.Application.Test.Common;

namespace ActvShare.Application.Test.UserManagement.Queries;
public class GetNotificationQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetNotificationQueryHandler _handler;

    public GetNotificationQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetNotificationQueryHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFoundError_When_UserNotFound()
    {
        // Arrange
        var query = new GetNotificationQuery(UserId.CreateUnique().Value);
        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(default(User));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.User.UserNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnNoNotificationsFoundError_When_UserHasNoNotifications()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var query = new GetNotificationQuery(user.Id.Value);

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.User.NoNotificationsFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnNotificationResponseList_When_UserHasNotifications()
    {
        // Arrange
        var msg = "Test notification";
        var user = DummyUser.GetDummyUser();
        var query = new GetNotificationQuery(user.Id.Value);

        user.AddNotification(msg);
        user.AddNotification(msg+"2");

        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(user);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<List<NotificationResponse>>();
        var notificationResponses = result.Value;
        notificationResponses.Should().HaveCount(2);
        var notificationResponse = notificationResponses.First();
        notificationResponse.Message.Should().Be(msg);
        notificationResponse.IsRead.Should().Be(false);
    }
}