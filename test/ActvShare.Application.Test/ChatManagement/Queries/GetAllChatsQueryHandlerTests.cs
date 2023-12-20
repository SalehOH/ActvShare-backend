using ActvShare.Application.ChatManagement.Queries.GetAllChats;
using ActvShare.Application.Common.Interfaces.Persistance;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Application.ChatManagement.Responses;
using System;
using ActvShare.Domain.Users.ValueObjects;
using ActvShare.Domain.Chats;
using ActvShare.Application.Test.Common;

namespace ActvShare.Application.Test.ChatManagement.Queries;
public class GetAllChatsQueryHandlerTests
{
    private readonly Mock<IChatRepository> _chatRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetAllChatsQueryHandler _handler;

    public GetAllChatsQueryHandlerTests()
    {
        _chatRepositoryMock = new Mock<IChatRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetAllChatsQueryHandler(_chatRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnChatNotFoundError_When_NoChatsFound()
    {
        // Arrange
        var query = new GetAllChatsQuery(UserId.CreateUnique().Value);
        _chatRepositoryMock.Setup(
            x => x.GetAllChatsAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new List<Chat>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.Chat.ChatNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnViewChatResponseList_When_ChatsFound()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var userId = UserId.Create(user.Id.Value);

        var other1 = DummyUser.CreateUser("other1");
        var other1Id = UserId.Create(other1.Id.Value);

        var other2 = DummyUser.CreateUser("other2");
        var other2Id = UserId.Create(other2.Id.Value);

        var msg = "Hello";
        var chat1 = Chat.Create(userId, other1Id);
        chat1.AddMessage(msg, other1Id);

        var chat2 = Chat.Create(userId, other2Id);
        chat2.AddMessage(msg + 2, userId);

        var query = new GetAllChatsQuery(user.Id.Value);
        
 
        _userRepositoryMock.SetupSequence(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(other1)
                  .ReturnsAsync(other2);

        _chatRepositoryMock.Setup(
            x => x.GetAllChatsAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new List<Chat> { chat1, chat2 });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);
        result.Value.Should().BeOfType<List<ViewChatResponse>>();

        result.Value.First().ChatId.Should().Be(chat1.Id.Value);
        result.Value.First().User.Username.Should().Be(other1.Username);
        result.Value.First().LastMessage.Should().Be(msg);

        result.Value.Last().ChatId.Should().Be(chat2.Id.Value);
        result.Value.Last().User.Username.Should().Be(other2.Username);
        result.Value.Last().LastMessage.Should().Be(msg + 2);
    }
}