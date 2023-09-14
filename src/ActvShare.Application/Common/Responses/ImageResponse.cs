using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActvShare.Application.Common.Responses
{
    public sealed record ImageResponse(string FileName, string OriginalFileName, long FileSize);
}
