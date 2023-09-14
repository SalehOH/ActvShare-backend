using ActvShare.Domain.Common.Models;
using ActvShare.Domain.Posts.ValueObjects;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Domain.Posts.Entities;

public class Reply: Entity<ReplyId>
{
    public UserId UserId { get; init; }
    public string Content { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    
    private Reply(ReplyId replyId, PostId postId, string content, UserId userId)
        :base(replyId)
    {
        Content = content;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public static Reply Create(PostId postId, string content, UserId userId)
    {
        return new Reply(ReplyId.CreateUnique(), postId, content, userId);
    }
    private Reply() { }
}