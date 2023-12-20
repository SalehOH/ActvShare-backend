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
    private readonly GetChatQueryHandler _handler;

    public GetChatQueryHandlerTests()
    {
        _chatRepositoryMock = new Mock<IChatRepository>();
        _handler = new GetChatQueryHandler(_chatRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnChatNotFoundError_When_ChatNotFound()
    {
        // Arrange
        var query = new GetChatQuery(ChatId.CreateUnique().Value);
        
        _chatRepositoryMock.Setup(
            x => x.GetChatByIdAsync(It.IsAny<ChatId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(default (Chat));

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

        var query = new GetChatQuery(chat.Id.Value);

        _chatRepositoryMock.Setup(
            x => x.GetChatByIdAsync(It.IsAny<ChatId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(chat);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<List<MessageResponse>>();
        result.Value.Should().HaveCount(1);
        var messageResponse = result.Value.First();
        messageResponse.Content.Should().Be(msg);
        messageResponse.SenderId.Should().Be(userId.Value);
    }
}