using Microsoft.AspNetCore.Mvc;
using MediatR;
using ActvShare.Application.UserManagement.Commands.Follow;
using ActvShare.Domain.Users.ValueObjects;
using ActvShare.Application.UserManagement.Queries.GetFollowings;
using Microsoft.AspNetCore.Authorization;
using ActvShare.Application.UserManagement.Queries.GetSearchUser;
using ActvShare.Application.UserManagement.Queries.GetNotifications;
using ActvShare.Application.UserManagement.Commands.UnFollow;
using ActvShare.Application.UserManagement.Queries.GetUserPosts;

namespace ActvShare.WebApi.Controllers;
[ApiController]
[Route("user")]
public class UserManagementController : BaseController
{
    private readonly ISender _sender;
    public UserManagementController(ISender sender)
    {
        _sender = sender;
    }


    [HttpPost("follow/{username}")]
    public async Task<IActionResult> Follow(string username)
    {
        var userId = GetAuthenticatedUserId();
        var result = await _sender.Send(new FollowCommand(username, UserId.Create(userId)));

        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }

    [HttpPost("unfollow/{username}")]
    public async Task<IActionResult> UnFollow(string username)
    {
        var userId = GetAuthenticatedUserId();
        var result = await _sender.Send(new UnFollowCommand(username, UserId.Create(userId)));

        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }

    [AllowAnonymous]
    [HttpGet("{username}")]
    public async Task<IActionResult> GetUserPosts(string username)
    {
        var result = await _sender.Send(new GetUserPostsQuery(username));
        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }

    [HttpGet("followings")]
    public async Task<IActionResult> GetFollowings()
    {
        var userId = GetAuthenticatedUserId();
        var result = await _sender.Send(new GetFollowingsQuery(userId));

        return result.Match(
                success => Ok(success),
                error => Problem(error));
    }

    [AllowAnonymous]
    [HttpGet("/search/{username}")]
    public async Task<IActionResult> GetSearchedUser(string username)
    {

        Guid? userId = null;
        if (isAuthenticated())
        {
            userId = GetAuthenticatedUserId();
        }

        var result = await _sender.Send(new GetSearchUserQuery(userId, username));
        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }

    [HttpGet("notifications")]
    public async Task<IActionResult> GetNotifications()
    {
        var userId = GetAuthenticatedUserId();
        var result = await _sender.Send(new GetNotificationQuery(userId));

        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }
}
