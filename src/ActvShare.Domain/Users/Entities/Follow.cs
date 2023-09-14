using ActvShare.Domain.Common.Models;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Domain.Users.Entities;

public class Follow: Entity<FollowId>
{
    public UserId FollowedUserId { get; }
    public DateTime FollowedAt { get; }
    
    private Follow(FollowId id, UserId followedUserId) : base(id)
    {
        FollowedUserId = followedUserId;
        FollowedAt = DateTime.UtcNow;
    }
    
    public static Follow Create(UserId followedUserId)
    {
        return new(FollowId.CreateUnique(), followedUserId);
    }
    
    private Follow(){}
}