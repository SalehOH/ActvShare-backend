using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.Common.Responses;

namespace ActvShare.Application.PostManagement.Responses
{
    public sealed record ReplyResponse(Guid Id, string Content, UserResponse User , DateTime CreatedAt);
    
}
