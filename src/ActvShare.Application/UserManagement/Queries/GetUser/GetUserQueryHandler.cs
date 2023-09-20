using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Users;
using ActvShare.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Queries.GetUser;

public class GetUserQueryHandler: IRequestHandler<GetUserQuery, ErrorOr<User>>
{
    private readonly IUserRepository _userRepository;
    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<ErrorOr<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
        {
            return Errors.User.UserNotFound;
        }
        return user;
    }
}