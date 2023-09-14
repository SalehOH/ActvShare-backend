using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.Common.Responses;

namespace ActvShare.Application.ChatManagement.Responses
{
    public sealed record ViewChatResponse(Guid ChatId, UserResponse User, string? LastMessage, DateTime LastMessageSentAt);
    
}
