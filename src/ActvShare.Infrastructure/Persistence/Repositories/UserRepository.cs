using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Users;
using ActvShare.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ActvShare.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var usernameLowerCase = username.ToLower();
        return await _context.Users.FirstOrDefaultAsync(user => user.Username.ToLower() == usernameLowerCase);
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }


    public async Task<User?> GetUserByIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<bool> IsEmailUniqeAsync(string email, CancellationToken cancellationToken = default)
    {
        return !await _context.Users.AnyAsync(user => user.Email.ToLower() == email);
    }

    public async Task<bool> IsUsernameUniqeAsync(string username, CancellationToken cancellationToken = default)
    {
        return !await _context.Users.AnyAsync(user => user.Username.ToLower() == username);
    }

    public async Task<List<User>> GetUsersByIdsAsync(List<UserId> userIds, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(user => userIds.Contains(user.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<User>> GetSearchedUserAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(user => user.Username.ToLower().Contains(username))
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.RefreshToken == refreshToken);
    }
}