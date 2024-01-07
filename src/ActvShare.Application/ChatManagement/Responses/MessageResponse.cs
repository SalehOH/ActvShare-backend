using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActvShare.Application.ChatManagement.Responses
{
    public sealed record MessageResponse(Guid Id, string Content, string Sender, DateTime CreatedAt);
}
