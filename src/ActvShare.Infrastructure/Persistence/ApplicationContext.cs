using ActvShare.Domain.Chats;
using ActvShare.Domain.Common.Models;
using ActvShare.Domain.Posts;
using ActvShare.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ActvShare.Infrastructure.Persistence;

public class ApplicationContext: DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Chat> Chats { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    
}