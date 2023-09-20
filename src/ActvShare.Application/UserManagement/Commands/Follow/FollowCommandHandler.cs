using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Commands.Follow;

public class FollowCommandHandler: IRequestHandler<FollowCommand, ErrorOr<FollowResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FollowCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<FollowResponse>> Handle(FollowCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
        {
            return Errors.User.UserNotFound;
        }

        var isFollowedSuccessfully = user.FollowUser(request.UserId);
        if (isFollowedSuccessfully is not true)
        {
            return Errors.User.UserIsAlreadyFollowed;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new FollowResponse(user.Name, user.Username, user.ProfileImage.StoredFileName);
    }
}