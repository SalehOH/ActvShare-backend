using ActvShare.Application.Common.Responses;

namespace ActvShare.Application.ChatManagement.Responses;
public sealed record ChatResponse(UserResponse User, List<MessageResponse> Messages);