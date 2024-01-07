using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.Common.Helpers;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.Common.Responses;
using ActvShare.Application.PostManagement.Responses;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Posts;
using ActvShare.Domain.Users.ValueObjects;
using ActvShare.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.PostManagement.Commands.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, ErrorOr<PostResponse>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICreateImage _createImage;

        public CreatePostCommandHandler(IPostRepository postRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, ICreateImage createImage)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _createImage = createImage;
        }

        public async Task<ErrorOr<PostResponse>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(UserId.Create(request.UserId), cancellationToken);
            if (user is null)
            {
                return Errors.User.UserNotFound;
            }

            var post = Post.Create(UserId.Create(user.Id.Value), request.Content);
            post.AddLike(UserId.Create(user.Id.Value));

            if (request.ContentPicture is not null)
            {
                var postImage = await _createImage.Create(request.ContentPicture);
                post.AddPostImage(postImage.FileName, postImage.OriginalFileName, postImage.FileSize);
            }

            await _postRepository.AddPostAsync(post, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userInfo = new UserResponse(user.Name, user.Username, user.ProfileImage.StoredFileName);
            return new PostResponse(
                post.Id.Value,
                post.Content,
                post.PostImage?.StoredFileName,
                userInfo,
                true,
                post.Likes.Count,
                post.CreatedAt
                );

        }
    }
}
