using ActvShare.Domain.Common.Models;
using ActvShare.Domain.Posts.Entities;
using ActvShare.Domain.Posts.ValueObjects;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Domain.Posts;

public class Post : AggregateRoot<PostId, Guid>
{
    private readonly List<Reply> _replies = new();
    private readonly List<Like> _likes = new();
    
    public UserId UserId { get; init; }
    public string? Content { get; init; }
    public PostImage? PostImage { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; private set; }
    
    public IReadOnlyList<Reply> Replies => _replies.AsReadOnly();
    public IReadOnlyList<Like> Likes => _likes.AsReadOnly();
    
    private Post(
        PostId postId, 
        UserId userId, 
        string? content, 
        List<Reply>? replies = null,
        List<Like>? likes = null
        )
        : base(postId)
    {
        UserId = userId;
        Content = content;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        _replies = replies ?? new();
        _likes = likes ?? new();
    }
    
    public static Post Create(
        UserId userId, 
        string? content, 
        List<Reply>? replies = null,
        List<Like>? likes = null
        )
    {
        return new(PostId.CreateUnique(), userId, content, replies, likes);
    }
    public void AddPostImage(string StoredFileName, string OriginalFileName, long FileSize)
    {
        this.PostImage = PostImage.Create(StoredFileName, OriginalFileName, null, FileSize);
    }
    public Reply AddReply(string content, UserId userId)
    {
        var reply = Reply.Create(PostId.Create(Id.Value), content, userId);
        _replies.Add(reply);

        return reply;
    }
    
    public void AddLike(UserId userId)
    {
        var like = Like.Create(userId);
        _likes.Add(like);
    }
    
    public void RemoveLike(UserId userId)
    {
        var like = _likes.FirstOrDefault(x => x.UserId == userId);
        if (like is not null)
        {
            _likes.Remove(like);
        }
    }

    private Post() { }
    
}