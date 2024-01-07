using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Commands.UnFollow
{
    public class UnFollowCommandHandler : IRequestHandler<UnFollowCommand, ErrorOr<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnFollowCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<bool>> Handle(UnFollowCommand request, CancellationToken cancellationToken)
        {
            var unFollowedUser = await _userRepository.GetUserByUsernameAsync(request.Username, cancellationToken);
            
            if (unFollowedUser is null)
                return Errors.User.UserNotFound;
            
            var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return Errors.User.UserNotFound;

            var isUnfollowedSuccessfully = user.UnfollowUser(UserId.Create(unFollowedUser.Id.Value));
            if (isUnfollowedSuccessfully is not true)
                return Errors.User.UserIsNotFollowed;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
