using ActvShare.Application.PostManagement.Responses;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ActvShare.Application.PostManagement.Commands.CreatePost
{
    public record CreatePostCommand(Guid UserId, string? Content, IFormFile? ContentPicture): IRequest<ErrorOr<PostResponse>>;
}
