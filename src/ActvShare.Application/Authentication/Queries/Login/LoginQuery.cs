using ActvShare.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.Authentication.Queries.Login
{
    public record LoginQuery(string Username, string Password) : IRequest<ErrorOr<AuthenticationResult>>;
}
