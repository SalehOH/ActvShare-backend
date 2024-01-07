using ActvShare.Application.Authentication.Common;
using ActvShare.Application.Common.Interfaces.Authentication;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.Authentication.Commands.RefreshToken;

public class RegisterCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByRefreshTokenAsync(request.RefreshToken);

        if (user is null)
            return Errors.Authentication.RefreshTokenNotFound;

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            user.Name,
            user.Username, 
            user.ProfileImage.StoredFileName, 
            user.Follows.Count, 
            token
        );
    }
}
