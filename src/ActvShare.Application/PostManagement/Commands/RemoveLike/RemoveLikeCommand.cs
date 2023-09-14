using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.PostManagement.Commands.DislikePost
{
    public sealed record RemoveLikeCommand(Guid PostId, Guid UserId) : IRequest<ErrorOr<bool>>;
    
}
