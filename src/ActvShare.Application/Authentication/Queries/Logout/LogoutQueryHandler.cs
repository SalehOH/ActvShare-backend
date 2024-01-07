using ActvShare.Application.Common.Interfaces.Persistance;
using ErrorOr;
using MediatR;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Application.Authentication.Queries.Logout;

public class LogoutQueryHandler : IRequestHandler<LogoutQuery, ErrorOr<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<bool>> Handle(LogoutQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(UserId.Create(request.UserId), cancellationToken);
        if (user is null)
        {
            return Errors.User.UserNotFound;
        }

        user.UpdateRefreshToken(null);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}