using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.Common.Responses;

namespace ActvShare.Application.PostManagement.Responses
{
    public record PostResponse(
        Guid Id,
        string? Content,
        string? ContentPicture,
        UserResponse UserResponse,
        bool IsLiked,
        int LikesCount,
        DateTime CreatedAt
        );
}
