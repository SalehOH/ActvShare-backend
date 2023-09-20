using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.Common.Responses;
using ActvShare.Application.PostManagement.Responses;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Posts.ValueObjects;
using ActvShare.Domain.Users.ValueObjects;
using ActvShare.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.PostManagement.Commands.CreateReply
{
    public class CreateReplyCommandHandler : IRequestHandler<CreateReplyCommand, ErrorOr<ReplyResponse>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public CreateReplyCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async Task<ErrorOr<ReplyResponse>> Handle(CreateReplyCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetPostByIdAsync(PostId.Create(request.PostId), cancellationToken);
            if (post is null)
                return Errors.Post.PostNotFound;

            var reply = post.AddReply(request.Content, UserId.Create(request.UserId));
            
            var user = await _userRepository.GetUserByIdAsync(reply.UserId, cancellationToken);
            if (user is null)
                return Errors.User.UserNotFound;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ReplyResponse(reply.Id.Value, reply.Content, new UserResponse(user.Name, user.Username, user.ProfileImage.OriginalFileName), reply.CreatedAt);
        }
    }
}
