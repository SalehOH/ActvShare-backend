using ActvShare.Application.Authentication.Common;
using ActvShare.Application.Common.Helpers;
using ActvShare.Application.Common.Interfaces.Authentication;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Users;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResultWithRefreshToken>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ICreateImage _createImage;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public RegisterCommandHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator,
        ICreateImage createImage,
        IRefreshTokenGenerator refreshTokenGenerator
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
        _createImage = createImage;
        _refreshTokenGenerator = refreshTokenGenerator;

    }

    public async Task<ErrorOr<AuthenticationResultWithRefreshToken>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var isEmailUnique = await _userRepository.IsEmailUniqeAsync(request.Email.ToLower(), cancellationToken);
        if (!isEmailUnique)
            return Errors.User.DuplicateEmail;


        var isUsernameUnique = await _userRepository.IsUsernameUniqeAsync(request.Username.ToLower(), cancellationToken);
        if (isUsernameUnique is not true)
            return Errors.User.DuplicateUsername;


        var HashedPassword = new PasswordHashing().HashPassword(request.Username, request.Password);
        var user = User.Create(request.Name, request.Username, request.Email, HashedPassword);

        var createImage = await _createImage.Create(request.ProfileImage);
        user.AddProfileImage(createImage.FileName, createImage.OriginalFileName, null, createImage.FileSize);

        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();
        user.UpdateRefreshToken(refreshToken);

        await _userRepository.AddUserAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user);
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