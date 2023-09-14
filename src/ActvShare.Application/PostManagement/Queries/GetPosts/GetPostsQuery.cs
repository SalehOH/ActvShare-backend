using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.PostManagement.Responses;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.PostManagement.Queries.GetPosts
{
    public sealed record GetPostsQuery(Guid? UserId) : IRequest<ErrorOr<List<PostResponse>>>;
}
