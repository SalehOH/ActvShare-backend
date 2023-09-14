using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.ChatManagement.Queries.GetChat
{
    internal class GetChatQueryValidator: AbstractValidator<GetChatQuery>
    {
        public GetChatQueryValidator()
        {
            RuleFor(x => x.ChatId).NotEmpty();
        }
    }
}
