using ActvShare.Application.Authentication.Common;
using MediatR;
using ErrorOr;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.Common.Interfaces.Authentication;
using ActvShare.Domain.Common.Errors;

namespace ActvShare.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        
        public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var user = await _userRepository.GetUserByUsernameAsync(request.Username, cancellationToken);
            if (user is null)
                return Errors.Authentication.InvalidCredentials;
            
            
            var result = new PasswordHashing().VerifyHashedPassword(request.Username, user!.Password, request.Password);
            if (result == 0)
                return Errors.Authentication.InvalidCredentials;

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
}
