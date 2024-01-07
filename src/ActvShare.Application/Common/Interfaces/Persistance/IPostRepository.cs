using ActvShare.Domain.Posts;
using ActvShare.Domain.Posts.ValueObjects;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Application.Common.Interfaces.Persistance
{
    public interface IPostRepository
    {
        Task<Post?> GetPostByIdAsync(PostId postId, CancellationToken cancellationToken = default);
        Task AddPostAsync(Post post, CancellationToken cancellationToken = default);
        Task<List<Post>> GetAllPostsAsync(CancellationToken cancellationToken = default);
        Task<List<Post>> GetPostsByUserAsync(UserId userId, CancellationToken cancellationToken = default);
    }
}
