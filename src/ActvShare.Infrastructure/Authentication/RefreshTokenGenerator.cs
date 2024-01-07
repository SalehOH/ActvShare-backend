using System.Security.Cryptography;
using ActvShare.Application.Common.Interfaces.Authentication;

namespace ActvShare.Infrastructure.Authentication;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string GenerateRefreshToken()
    {
        var token = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(token);
    }
}