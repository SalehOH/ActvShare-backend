namespace ActvShare.Application.Authentication.Common;

public record AuthenticationResult(
   string Name, string Username, string ProfilePicture, int FollowsCount, string Token);