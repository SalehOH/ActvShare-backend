namespace ActvShare.Application.Authentication.Common;
public record AuthenticationResultWithRefreshToken(AuthenticationResult user, string RefreshToken);
