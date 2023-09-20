using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.Common.Responses;
using Microsoft.AspNetCore.Http;

namespace ActvShare.Application.Common.Helpers
{
    public interface ICreateImage
    {
        Task<ImageResponse> Create(IFormFile file, CancellationToken cancellationToken = default);
    }
}
