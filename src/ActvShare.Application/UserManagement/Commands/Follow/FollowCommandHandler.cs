using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Users.ValueObjects;
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
        var followedUser = await _userRepository.GetUserByUsernameAsync(request.Username, cancellationToken);
        if (followedUser is null)
        {
            return Errors.User.UserNotFound;
        }
        
        var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Errors.User.UserNotFound;
        }
        
        var isFollowedSuccessfully = user.FollowUser(UserId.Create(followedUser.Id.Value));

        if (isFollowedSuccessfully is not true)
        {
            return Errors.User.UserIsAlreadyFollowed;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new FollowResponse(followedUser.Name, followedUser.Username, followedUser.ProfileImage.StoredFileName, false);
    }
}