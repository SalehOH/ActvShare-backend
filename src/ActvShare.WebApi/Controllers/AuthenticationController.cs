using ActvShare.Application.Authentication.Commands.RefreshToken;
using ActvShare.Application.Authentication.Commands.Register;
using ActvShare.Application.Authentication.Queries.Login;
using ActvShare.Application.Authentication.Queries.Logout;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActvShare.WebApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : BaseController
{
    private readonly ISender _sender;
    public AuthenticationController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.Match(
            success =>
            {
                SetRefreshTokenCookie(success.RefreshToken);
                return StatusCode(StatusCodes.Status201Created, success.user);
            },
            error => Problem(error));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.Match(
            success =>
            {
                SetRefreshTokenCookie(success.RefreshToken);
                return Ok(success.user);
            },
            error => Problem(error));
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken is null)
            return Unauthorized();

        var command = new RefreshTokenCommand(refreshToken);
        var result = await _sender.Send(command, cancellationToken);
        
        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken is null)
            return Unauthorized();

        var userId = GetAuthenticatedUserId();
        var result = await _sender.Send(new LogoutQuery(userId), cancellationToken);

        Response.Cookies.Delete("refreshToken");
        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

}