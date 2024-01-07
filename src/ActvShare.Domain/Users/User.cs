using ActvShare.Domain.Common.Models;
using ActvShare.Domain.Users.Entities;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Domain.Users;

public class User : AggregateRoot<UserId, Guid>
{
    private readonly List<Follow> _follows = new();
    private readonly List<Notification> _notifications = new();

    public string Name { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public ProfileImage ProfileImage { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime LastLogin { get; private set; }
    public string? RefreshToken { get; private set; }
    public bool IsEmailVerified { get; private set; }

    public IReadOnlyCollection<Follow> Follows => _follows.AsReadOnly();
    public IReadOnlyCollection<Notification> Notifications => _notifications.AsReadOnly();

    private User(
        UserId userId,
        string name,
        string username,
        string email,
        string password,
        string? refreshToken,
        bool isEmailVerified = false
        )
        : base(userId)
    {
        Name = name;
        Username = username;
        Email = email;
        Password = password;
        CreatedAt = DateTime.UtcNow;
        LastLogin = DateTime.UtcNow;
        RefreshToken = refreshToken;
        IsEmailVerified = isEmailVerified;
        _follows = new();
    }

    public bool FollowUser(UserId followedUserId)
    {
        if (Follows.Any(x => x.FollowedUserId == followedUserId))
        {
            return false;
        }

        var follow = Follow.Create(followedUserId);
        _follows.Add(follow);

        return true;
    }

    public bool UnfollowUser(UserId followedUserId)
    {
        var follow = Follows.FirstOrDefault(x => x.FollowedUserId == followedUserId);
        if (follow is not null)
        {
            _follows.Remove(follow);
            return true;
        }
        return false;
    }
    public void AddNotification(string message)
    {
        var notification = Notification.Create(message);
        _notifications.Add(notification);
    }
    public void UpdateRefreshToken(string? refreshToken)
    {
        RefreshToken = refreshToken;
    }
    public void AddProfileImage(string storedFileName, string originalFileName, string? contentType, long fileSize)
    {
        ProfileImage = ProfileImage.Create(
            storedFileName,
            originalFileName,
            contentType,
            fileSize);
    }
    public static User Create(
        string name,
        string username,
        string email,
        string password,
        string? refreshToken = null,
        bool isEmailVerified = false)
    {
        return new User(
            UserId.CreateUnique(),
            name,
            username,
            email,
            password,
            refreshToken,
            isEmailVerified);
    }
    private User() { }

}