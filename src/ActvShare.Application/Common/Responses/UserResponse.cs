using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActvShare.Application.Common.Responses
{
    public sealed record UserResponse(string Name, string Username, string ProfilePicture);
  
}
