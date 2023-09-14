using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.PostManagement.Responses;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.PostManagement.Commands.CreateReply
{
    public record CreateReplyCommand(Guid PostId, string Content, Guid UserId): IRequest<ErrorOr<ReplyResponse>>;
  
}
