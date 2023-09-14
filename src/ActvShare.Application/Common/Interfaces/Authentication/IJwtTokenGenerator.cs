using ActvShare.Domain.Users;

namespace ActvShare.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}