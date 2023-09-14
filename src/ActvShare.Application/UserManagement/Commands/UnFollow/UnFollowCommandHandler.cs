using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Abstractions;
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
                return Error.Conflict("User not found");

            user.UnfollowUser(request.UserId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new FollowResponse(user.Name, user.Username, user.ProfileImage.StoredFileName);
        }
    }
}
