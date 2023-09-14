using ActvShare.Domain.Posts;
using ActvShare.Domain.Posts.ValueObjects;

namespace ActvShare.Application.Common.Interfaces.Persistance
{
    public interface IPostRepository
    {
        Task<Post?> GetPostByIdAsync(PostId postId, CancellationToken cancellationToken = default);
        Task AddPostAsync(Post post, CancellationToken cancellationToken = default);
        Task<List<Post>> GetAllPostsAsync(CancellationToken cancellationToken = default);
    }
}
