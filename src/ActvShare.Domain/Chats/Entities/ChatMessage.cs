using ActvShare.Domain.Chats.ValueObjects;
using ActvShare.Domain.Common.Models;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Domain.Chats.Entities;

public class ChatMessage: Entity<ChatMessageId>
{
    public string Content { get; init; }
    public UserId SenderId { get; init; }
    public DateTime SentAt { get; init; }
    
    private ChatMessage(ChatMessageId id, string content, UserId senderId) : base(id)
    {
        Content = content;
        SenderId = senderId;
        SentAt = DateTime.UtcNow;
    }
    
    public static ChatMessage Create(ChatId chatId, string content,  UserId senderId)
    {
        return new(ChatMessageId.CreateUnique(), content, senderId);
    }
    
    private ChatMessage(){}
}