using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.ChatManagement.Commands.CreateChat
{
    public sealed record CreateChatCommand(Guid UserId, string OtherUser): IRequest<ErrorOr<bool>>;
}
