using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Posts;
using ActvShare.Domain.Posts.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ActvShare.Infrastructure.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationContext _context;

        public PostRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddPostAsync(Post post, CancellationToken cancellationToken = default)
        {
            await _context.Posts.AddAsync(post, cancellationToken);
        }

        public async Task<List<Post>> GetAllPostsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Posts.ToListAsync(cancellationToken);
        }

        public async Task<Post?> GetPostByIdAsync(PostId postId, CancellationToken cancellationToken = default)
        {
            return await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
        }
    }
}
