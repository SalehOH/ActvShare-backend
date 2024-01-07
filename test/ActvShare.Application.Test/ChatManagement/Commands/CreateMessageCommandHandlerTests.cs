using ActvShare.Application.ChatManagement.Commands.CreateMessage;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Chats.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Users.ValueObjects;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Chats;
using ActvShare.Application.Test.Common;
using ActvShare.Application.ChatManagement.Responses;

namespace ActvShare.Application.Test.ChatManagement.Commands;
public class CreateMessageCommandHandlerTests
{
    private readonly Mock<IChatRepository> _chatRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateMessageCommandHandler _handler;

    public CreateMessageCommandHandlerTests()
    {
        _chatRepositoryMock = new Mock<IChatRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateMessageCommandHandler(_chatRepositoryMock.Object, _userRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnChatNotFoundError_When_ChatNotFound()
    {
        // Arrange
        var command = new CreateMessageCommand(ChatId.CreateUnique().Value, UserId.CreateUnique().Value, "Hello");

        _chatRepositoryMock.Setup(
            x => x.GetChatByIdAsync(It.IsAny<ChatId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(default(Chat));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.Chat.ChatNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnMessageResponse_When_MessageCreated()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();
        var userId = UserId.Create(user.Id.Value);
        var chat = Chat.Create(userId, userId);

        var command = new CreateMessageCommand(chat.Id.Value, userId.Value, "Hello");

        _chatRepositoryMock.Setup(
            x => x.GetChatByIdAsync(It.IsAny<ChatId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(chat);
        
        _userRepositoryMock.Setup(
            x => x.GetUserByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(user);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<MessageResponse>();
        result.Value.Content.Should().Be(command.Content);
        result.Value.Sender.Should().Be(user.Username);
    }
}