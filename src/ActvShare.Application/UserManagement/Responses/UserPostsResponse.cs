namespace ActvShare.Application.UserManagement.Responses;
public sealed record UserPostsResponse(SearchUserResponse User, IEnumerable<PostResponse> Posts);

public sealed record PostResponse(Guid Id,
    string? Content,
    string? ContentPicture,
    bool IsLiked,
    int LikesCount,
    DateTime CreatedAt);