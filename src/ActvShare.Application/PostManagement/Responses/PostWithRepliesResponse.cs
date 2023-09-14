using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActvShare.Application.PostManagement.Responses
{
    public sealed record PostWithRepliesResponse(PostResponse Post, List<ReplyResponse> Replies);
    
}
