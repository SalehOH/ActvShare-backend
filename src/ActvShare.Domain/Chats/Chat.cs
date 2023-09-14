using ActvShare.Domain.Chats.Entities;
using ActvShare.Domain.Chats.ValueObjects;
using ActvShare.Domain.Common.Models;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Domain.Chats;

public class Chat: AggregateRoot<ChatId, Guid>
{
    private readonly List<ChatMessage> _messages = new();
    public UserId user1 { get; init; }
    public UserId user2 { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; private set; }
    
    public IReadOnlyList<ChatMessage> Messages => _messages.AsReadOnly();
    
    private Chat(
        ChatId chatId, 
        UserId user1, 
        UserId user2
        )
        : base(chatId)
    {
        this.user1 = user1;
        this.user2 = user2;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        _messages = new();
    }
    
    public static Chat Create(
        UserId user1, 
        UserId user2
        )
    {
        return new(ChatId.CreateUnique(), user1, user2);
    }
    
    public ChatMessage AddMessage(string content, UserId userId)
    {
        var message = ChatMessage.Create(ChatId.Create(Id.Value), content, userId);
        _messages.Add(message);
        UpdatedAt = DateTime.Now;

        return message;
    }
    private Chat() { }
}