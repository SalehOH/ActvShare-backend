using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;


namespace ActvShare.Application.UserManagement.Queries.GetUserPosts;

public class GetUserPostsQueryHandler : IRequestHandler<GetUserPostsQuery, ErrorOr<UserPostsResponse>>
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public GetUserPostsQueryHandler(IPostRepository postRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
    }
    public async Task<ErrorOr<UserPostsResponse>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
            return Errors.User.UserNotFound;

        var posts = await _postRepository.GetPostsByUserAsync(UserId.Create(user.Id.Value), cancellationToken);
        if (posts is null)
            return Errors.Post.PostNotFound;

        var userPosts = posts.Select(post =>
        {
            var IsLiked = post.Likes.Any(like => like.UserId == user.Id);
            return new PostResponse(post.Id.Value, post.Content, post.PostImage?.StoredFileName, IsLiked, post.Likes.Count, post.CreatedAt);
        }).ToList();

        return new UserPostsResponse(new SearchUserResponse(user.Name, user.Username, user.ProfileImage.StoredFileName, null, null), userPosts);
    }
}