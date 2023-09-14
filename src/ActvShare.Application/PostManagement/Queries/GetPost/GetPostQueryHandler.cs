using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.Common.Responses;
using ActvShare.Application.PostManagement.Responses;
using ActvShare.Domain.Posts.ValueObjects;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.PostManagement.Queries.GetPost
{
    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, ErrorOr<PostWithRepliesResponse>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public GetPostQueryHandler(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }
        public async Task<ErrorOr<PostWithRepliesResponse>> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetPostByIdAsync(PostId.Create(request.PostId), cancellationToken);
            if (post is null)
                return Error.NotFound("Post not found");
            
            var user = await _userRepository.GetUserByIdAsync(post.UserId, cancellationToken);
            var userResponse = new UserResponse(user!.Name, user.Username, user.ProfileImage.StoredFileName);
            var postResponse = new PostResponse(post.Id.Value, post.Content, post.PostImage?.StoredFileName, userResponse, post.Likes.Count, post.CreatedAt);

            var replies = await Task.WhenAll(post.Replies.Select(async reply =>
            {
                var user = await _userRepository.GetUserByIdAsync(reply.UserId, cancellationToken);
                return new ReplyResponse(reply.Id.Value, reply.Content, new UserResponse(user!.Name, user.Username, user.ProfileImage.StoredFileName), reply.CreatedAt);
            }));

            return new PostWithRepliesResponse(postResponse, replies.ToList());
        }
    }
}
