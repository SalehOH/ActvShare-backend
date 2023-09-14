using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.ChatManagement.Responses;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.ChatManagement.Commands.CreateMessage
{
    public sealed record CreateMessageCommand(Guid ChatId, Guid SenderId, string Content): IRequest<ErrorOr<MessageResponse>>;
}
