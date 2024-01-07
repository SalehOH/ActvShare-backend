namespace ActvShare.WebApi.Contracts;
public sealed record CreateReplyRequest(Guid PostId, string Content);