using ActvShare.Domain.Chats;
using ActvShare.Domain.Chats.ValueObjects;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Application.Common.Interfaces.Persistance
{
    public interface IChatRepository
    {
        Task<Chat?> GetChatByIdAsync(ChatId chatId, CancellationToken cancellationToken = default);
        Task AddChatAsync(Chat chat, CancellationToken cancellationToken = default);
        Task<List<Chat>> GetAllChatsAsync(UserId userId, CancellationToken cancellationToken = default);
        Task<bool> ChatExistsAsync(UserId userId, UserId otherUserId, CancellationToken cancellationToken = default);
    }
}
