using ActvShare.Domain.Users;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Application.Common.Interfaces.Persistance;

public interface IUserRepository
{
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqeAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> IsUsernameUniqeAsync(string username, CancellationToken cancellationToken = default);
    Task<List<User>> GetUsersByIdsAsync(List<UserId> userIds, CancellationToken cancellationToken = default);
    Task<List<User>> GetSearchedUserAsync(string usernames, CancellationToken cancellationToken = default);
    Task AddUserAsync(User user, CancellationToken cancellationToken = default);
}