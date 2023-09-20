using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.UserManagement.Commands.UnFollow
{
    public class UnFollowCommandValidator: AbstractValidator<UnFollowCommand>
    {
        public UnFollowCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
