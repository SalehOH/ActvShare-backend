using ActvShare.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.Authentication.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<ErrorOr<AuthenticationResult>>;