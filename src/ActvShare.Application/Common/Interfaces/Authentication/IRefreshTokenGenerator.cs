namespace ActvShare.Application.Common.Interfaces.Authentication;

public interface IRefreshTokenGenerator
{
    string GenerateRefreshToken();
}