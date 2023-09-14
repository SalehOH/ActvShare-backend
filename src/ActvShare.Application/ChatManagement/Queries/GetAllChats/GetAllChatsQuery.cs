﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.ChatManagement.Responses;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.ChatManagement.Queries.GetAllChats
{
    public sealed record GetAllChatsQuery(Guid UserId): IRequest<ErrorOr<List<ViewChatResponse>>>;
    
}
