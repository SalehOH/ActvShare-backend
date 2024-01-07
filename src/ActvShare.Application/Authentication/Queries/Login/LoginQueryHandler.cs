using ActvShare.Application.Authentication.Common;
using MediatR;
using ErrorOr;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.Common.Interfaces.Authentication;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Abstractions;

namespace ActvShare.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResultWithRefreshToken>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public LoginQueryHandler(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IUnitOfWork unitOfWork
            )
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<AuthenticationResultWithRefreshToken>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetUserByUsernameAsync(request.Username, cancellationToken);
            if (user is null)
                return Errors.Authentication.InvalidCredentials;

            var result = new PasswordHashing().VerifyHashedPassword(request.Username, user!.Password, request.Password);
            if (result == 0)
                return Errors.Authentication.InvalidCredentials;

            var token = _jwtTokenGenerator.GenerateToken(user);
            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();
            
            user.UpdateRefreshToken(refreshToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userResponse = new AuthenticationResult(
                user.Name,
                user.Username,
                user.ProfileImage.StoredFileName,
                user.Follows.Count,
                token
            );

            return new AuthenticationResultWithRefreshToken(userResponse, refreshToken);
        }
    }
}
