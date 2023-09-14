using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.ChatManagement.Queries.GetAllChats
{
    public class GetAllChatsQueryValidator: AbstractValidator<GetAllChatsQuery>
    {
        public GetAllChatsQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
