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
        return await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
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
        return !await _context.Users.AnyAsync(user => user.Email == email);
    }

    public async Task<bool> IsUsernameUniqeAsync(string username, CancellationToken cancellationToken = default)
    { 
        return !await _context.Users.AnyAsync(user => user.Username == username);
    }

    public async Task<List<User>> GetUsersByIdsAsync(List<UserId> userIds, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(user => userIds.Contains(user.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<User>> GetSearchedUserAsync(string usernames, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(user => user.Username.Contains(usernames))
            .ToListAsync(cancellationToken);
    }
}