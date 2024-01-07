using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Chats;
using ActvShare.Domain.Chats.ValueObjects;
using ActvShare.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ActvShare.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationContext _context;

        public ChatRepository(ApplicationContext context)
        {
               _context = context;
        }

        public async Task AddChatAsync(Chat chat, CancellationToken cancellationToken = default)
        {
            await _context.Chats.AddAsync(chat, cancellationToken);
        }

        public Task<bool> ChatExistsAsync(UserId userId, UserId otherUserId, CancellationToken cancellationToken = default)
        {
            return _context.Chats.AnyAsync(chat => 
                (chat.user1 == userId && (chat.user2 == otherUserId || chat.user1 == otherUserId)) 
                || (chat.user2 == userId && (chat.user1 == otherUserId || chat.user2 == otherUserId)), cancellationToken);
        }

        public async Task<List<Chat>> GetAllChatsAsync(UserId userId, CancellationToken cancellationToken = default)
        {
            return await _context.Chats.Where(chat => chat.user1 == userId || chat.user2 == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Chat?> GetChatByIdAsync(ChatId chatId, CancellationToken cancellationToken = default)
        {
            return await _context.Chats.FirstOrDefaultAsync(chat => chat.Id == chatId, cancellationToken);
        }
    }
}
