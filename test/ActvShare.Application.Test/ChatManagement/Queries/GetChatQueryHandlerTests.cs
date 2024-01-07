using ActvShare.Application.ChatManagement.Queries.GetChat;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Chats.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using System.Collections.Generic;
using ActvShare.Application.ChatManagement.Responses;
using System;
using ActvShare.Domain.Chats;
using ActvShare.Application.Test.Common;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Application.Test.ChatManagement.Queries;
public class GetChatQueryHandlerTests
{
    private readonly Mock<IChatRepository> _chatRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetChatQueryHandler _handler;

    public GetChatQueryHandlerTests()
    {
        _chatRepositoryMock = new Mock<IChatRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetChatQueryHandler(_chatRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnChatNotFoundError_When_ChatNotFound()
    {
        // Arrange
        var query = new GetChatQuery(ChatId.CreateUnique().Value, Guid.NewGuid());

        _chatRepositoryMock.Setup(
            x => x.GetChatByIdAsync(It.IsAny<ChatId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(default(Chat));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.Chat.ChatNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnMessageResponseList_When_ChatFound()
    {
        // Arrange

        var user = DummyUser.GetDummyUser();
        var userId = UserId.Create(user.Id.Value);

        var otherUser = DummyUser.CreateUser("janedoe");
        var otherUserId = UserId.Create(otherUser.Id.Value);

        var chat = Chat.Create(userId, otherUserId);
        var msg = "Hello";
        chat.AddMessage(msg, userId);

        var query = new GetChatQuery(chat.Id.Value, Guid.NewGuid());

        _chatRepositoryMock.Setup(
            x => x.GetChatByIdAsync(It.IsAny<ChatId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(chat);

        _userRepositoryMock.SetupSequence(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(user)
                  .ReturnsAsync(otherUser);
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Messages.Should().BeOfType<List<MessageResponse>>();
        result.Value.Messages.Should().HaveCount(1);
        var messageResponse = result.Value.Messages.First();
        messageResponse.Content.Should().Be(msg);
        messageResponse.Sender.Should().Be(user.Username);
    }
}