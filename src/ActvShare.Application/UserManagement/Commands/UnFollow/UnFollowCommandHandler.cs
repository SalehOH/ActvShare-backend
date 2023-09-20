using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Commands.UnFollow
{
    public class UnFollowCommandHandler : IRequestHandler<UnFollowCommand, ErrorOr<FollowResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnFollowCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<FollowResponse>> Handle(UnFollowCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.Username, cancellationToken);
            
            if (user is null)
                return Errors.User.UserNotFound;

            var isUnfollowedSuccessfully = user.UnfollowUser(request.UserId);
            if (isUnfollowedSuccessfully is not true)
                return Errors.User.UserIsNotFollowed;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new FollowResponse(user.Name, user.Username, user.ProfileImage.StoredFileName);
        }
    }
}
