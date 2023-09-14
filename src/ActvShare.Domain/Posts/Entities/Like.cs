using ActvShare.Domain.Common.Models;
using ActvShare.Domain.Posts.ValueObjects;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Domain.Posts.Entities;

public class Like: Entity<LikeId>
{
    public UserId UserId { get; init; }
    public DateTime CreatedAt { get; init; }

    private Like(LikeId likeId, UserId userId):base(likeId)
    {
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }
    
    public static Like Create(UserId userId)
    {
        return new Like(LikeId.CreateUnique(), userId);
    }
    private Like() { }
}