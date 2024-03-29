﻿using ActvShare.Application.PostManagement.Responses;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.PostManagement.Queries.GetPost
{
    public record GetPostQuery(Guid PostId, Guid? UserId): IRequest<ErrorOr<PostWithRepliesResponse>>;
}
