using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.PostManagement.Commands.LikePost
{
    public sealed record LikePostCommand(Guid PostId, Guid UserId) : IRequest<ErrorOr<bool>>;
}
