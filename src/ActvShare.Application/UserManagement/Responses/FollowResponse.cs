using ActvShare.Domain.Users;

namespace ActvShare.Application.UserManagement.Responses;

public record FollowResponse(string Name, string Username, string ProfilePicture, bool hasChat);