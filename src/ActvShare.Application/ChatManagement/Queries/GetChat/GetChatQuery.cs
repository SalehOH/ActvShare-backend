using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.ChatManagement.Responses;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.ChatManagement.Queries.GetChat
{
    public sealed record GetChatQuery(Guid ChatId) : IRequest<ErrorOr<List<MessageResponse>>> ;
   
}
