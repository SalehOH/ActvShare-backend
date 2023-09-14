using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActvShare.Application.UserManagement.Responses
{
    public sealed record SearchUserResponse(string Name, string Username, string ProfilePicture);
}
