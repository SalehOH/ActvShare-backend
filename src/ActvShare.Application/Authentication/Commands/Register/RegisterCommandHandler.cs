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

public class RegisterCommandHandler: IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ICreateImage _createImage;

    public RegisterCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator, ICreateImage createImage)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
        _createImage = createImage;

    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var isEmailUnique = await _userRepository.IsEmailUniqeAsync(request.Email, cancellationToken);
        if (!isEmailUnique)
            return Errors.User.DuplicateEmail;
    
        
        var isUsernameUnique = await _userRepository.IsUsernameUniqeAsync(request.Username, cancellationToken);
        if (!isUsernameUnique)
            return Errors.User.DuplicateUsername;


        var HashedPassword = new PasswordHashing().HashPassword(request.Username, request.Password);
        var user = User.Create(request.Name, request.Username, request.Email, HashedPassword);

        var createImage = await _createImage.Create(request.File);
        user.AddProfileImage(createImage.FileName, createImage.OriginalFileName, null, createImage.FileSize);
        
        await _userRepository.AddUserAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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