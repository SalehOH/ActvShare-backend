using ActvShare.Domain.Users;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Queries.GetUser;

public record GetUserQuery(string Username): IRequest<ErrorOr<User>>;