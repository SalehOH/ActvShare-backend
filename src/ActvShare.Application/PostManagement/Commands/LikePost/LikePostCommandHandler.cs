using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Posts.ValueObjects;
using ActvShare.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.PostManagement.Commands.LikePost
{
    public class LikePostCommandHandler : IRequestHandler<LikePostCommand, ErrorOr<bool>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LikePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async Task<ErrorOr<bool>> Handle(LikePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetPostByIdAsync(PostId.Create(request.UserId), cancellationToken);
            if (post is null)
            {
                return Error.Validation("Post not found");
            }
            
            post.AddLike(UserId.Create(request.UserId));
            var creator = await _userRepository.GetUserByIdAsync(post.UserId, cancellationToken);
            
            creator!.AddNotification($"your post was liked {post.Content?.Split(' ')[0] ?? ""}");
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
