using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Commands.Follow;

public record FollowCommand(string Username, UserId UserId) : IRequest<ErrorOr<FollowResponse>>;
