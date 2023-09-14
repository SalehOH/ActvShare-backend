using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.PostManagement.Commands.DislikePost;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Posts.ValueObjects;
using ActvShare.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.PostManagement.Commands.RemoveLike
{
    public class RemoveLikeCommandHandler : IRequestHandler<RemoveLikeCommand, ErrorOr<bool>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveLikeCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(RemoveLikeCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetPostByIdAsync(PostId.Create(request.UserId), cancellationToken);
            if (post is null)
            {
                return Error.Validation("Post not found");
            }

            post.RemoveLike(UserId.Create(request.UserId));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
