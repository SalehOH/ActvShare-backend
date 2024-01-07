using ActvShare.Application.Authentication.Common;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ActvShare.Application.Authentication.Commands.Register;

public record RegisterCommand(string Name, string Username, string Email, string Password, string ConfirmPassowrd, IFormFile ProfileImage) 
    : IRequest<ErrorOr<AuthenticationResultWithRefreshToken>>;