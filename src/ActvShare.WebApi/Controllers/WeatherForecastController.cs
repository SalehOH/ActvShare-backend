using ActvShare.Application.Authentication.Commands.Register;
using ActvShare.Application.Authentication.Queries.Login;
using ActvShare.Application.UserManagement.Commands.Follow;
using ActvShare.Application.UserManagement.Queries.GetFollowings;
using ActvShare.Application.UserManagement.Queries.GetUser;
using ActvShare.Domain.Users.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
 
    private readonly ISender _sender;
    public WeatherForecastController( ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("/api/auth/register")]
    public async Task<IActionResult> Register([FromForm]RegisterCommand command)
    {
        var result = await _sender.Send(command);
        return result.Match<IActionResult>(
            success => Ok(success),
            error => BadRequest(error));
    }

    [HttpPost("/api/auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery command)
    {
        var result = await _sender.Send(command);
        return result.Match<IActionResult>(
                success => Ok(success),
                error => BadRequest(error));
    }

    [HttpPost("/follow/{id}")]
    public async Task<IActionResult> Follow(Guid id, string username)
    {
        var result = await _sender.Send(new FollowCommand(username, UserId.Create(id)));
        return result.Match<IActionResult>(
            success => Ok(success),
            error => BadRequest(error));
    }
    
    [HttpGet("/user/{username}")]
    public async Task<IActionResult> GetUser(string username)
    {
        var result = await _sender.Send(new GetUserQuery(username));
        return result.Match<IActionResult>(
            success => Ok(success),
            error => BadRequest(error));
    }

    [HttpGet("/user/followings/{username}")]
    public async Task<IActionResult> GetFollowings(Guid userId)
    {
        var result = await _sender.Send(new GetFollowingsQuery(userId));
        
        return result.Match<IActionResult>(
                success => Ok(success),
                error => BadRequest(error));
    }
}