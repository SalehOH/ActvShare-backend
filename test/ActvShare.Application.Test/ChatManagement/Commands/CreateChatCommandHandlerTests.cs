using ActvShare.Application.ChatManagement.Commands.CreateChat;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Users.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Chats.ValueObjects;
using ActvShare.Application.Test.Common;

namespace ActvShare.Application.Test.ChatManagement.Commands;
public class CreateChatCommandHandlerTests
{
    private readonly Mock<IChatRepository> _chatRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateChatCommandHandler _handler;

    public CreateChatCommandHandlerTests()
    {
        _chatRepositoryMock = new Mock<IChatRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateChatCommandHandler(_chatRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnChatAlreadyExistsError_When_ChatExists()
    {
        // Arrange
        var command = new CreateChatCommand(ChatId.CreateUnique().Value, UserId.CreateUnique().Value);
        _chatRepositoryMock.Setup(
            x => x.ChatExistsAsync(It.IsAny<UserId>(), It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.First().Should().Be(Errors.Chat.ChatAlreadyExists);
    }

    [Fact]
    public async Task Handle_Should_ReturnTrue_When_ChatCreated()
    {
        // Arrange
        var user = DummyUser.GetDummyUser();

        var command = new CreateChatCommand(user.Id.Value, user.Id.Value);
        _chatRepositoryMock.Setup(
            x => x.ChatExistsAsync(It.IsAny<UserId>(), It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeTrue();
    }
}