using ErrorOr;
using MediatR;

namespace ActvShare.Application.Authentication.Queries.Logout;

public sealed record LogoutQuery(Guid UserId) : IRequest<ErrorOr<bool>>;