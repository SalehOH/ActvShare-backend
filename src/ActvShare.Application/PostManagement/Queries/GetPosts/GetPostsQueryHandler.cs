using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.Common.Responses;
using ActvShare.Application.PostManagement.Responses;
using ActvShare.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.PostManagement.Queries.GetPosts
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, ErrorOr<List<PostResponse>>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public GetPostsQueryHandler(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<List<PostResponse>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllPostsAsync(cancellationToken);
            
            if ( posts.Any() is not true )
                return Errors.Post.PostNotFound;

            var userIds = posts.Select(post => post.UserId).Distinct().ToList();
            var users = await _userRepository.GetUsersByIdsAsync(userIds, cancellationToken);

            var postResponses = posts.Select(post =>
            {
                var postCreator = users.FirstOrDefault(user => user.Id == post.UserId);
                
                var createrResponse = new UserResponse(
                    postCreator!.Name,
                    postCreator.Username,
                    postCreator.ProfileImage.StoredFileName);

                return new PostResponse(
                    post.Id.Value,
                    post.Content,
                    post.PostImage?.OriginalFileName,
                    createrResponse,
                    post.Likes.Count,
                    post.CreatedAt);
            }).ToList();

            return postResponses;
        }
    }
}
