using Microsoft.AspNetCore.Mvc;
using MediatR;
using ActvShare.Application.PostManagement.Commands.CreatePost;
using ActvShare.Application.PostManagement.Queries.GetPost;
using ActvShare.Application.PostManagement.Queries.GetPosts;
using Microsoft.AspNetCore.Authorization;
using ActvShare.Application.PostManagement.Commands.LikePost;
using ActvShare.Application.PostManagement.Commands.RemoveLike;
using ActvShare.Application.PostManagement.Commands.CreateReply;
using ActvShare.WebApi.Contracts;

namespace ActvShare.WebApi.Controllers;

[ApiController]
[Route("post")]
public class PostManagementController : BaseController
{
    private readonly ISender _sender;
    public PostManagementController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromForm] string? Content, IFormFile? ContentPicture, CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();
        var command = new CreatePostCommand(userId, Content, ContentPicture);
        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }

    [AllowAnonymous]
    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPost(Guid postId, CancellationToken cancellationToken)
    {
        Guid? userId = null;
        if (isAuthenticated())
            userId = GetAuthenticatedUserId();

        var query = new GetPostQuery(postId, userId);
        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetPosts(CancellationToken cancellationToken)
    {
        Guid? userId = null;
        if (isAuthenticated())
            userId = GetAuthenticatedUserId();

        var result = await _sender.Send(new GetPostsQuery(userId), cancellationToken);

        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }

    [HttpPost("reply")]
    public async Task<IActionResult> CreateReply([FromBody] CreateReplyRequest request, CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();
        var command = new CreateReplyCommand(request.PostId, request.Content, userId);
        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }
    
    [HttpPost("like/{postId}")]
    public async Task<IActionResult> LikePost(Guid postId, CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();
        var command = new LikePostCommand(postId, userId);
        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }

    [HttpDelete("like/{postId}")]
    public async Task<IActionResult> RemoveLikePost(Guid postId, CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();
        var command = new RemoveLikeCommand(postId, userId);
        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }
}