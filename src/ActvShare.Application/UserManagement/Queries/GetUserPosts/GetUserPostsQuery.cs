using ActvShare.Application.UserManagement.Responses;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Queries.GetUserPosts;
public sealed record GetUserPostsQuery(string Username) : IRequest<ErrorOr<UserPostsResponse>>;