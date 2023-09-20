using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.PostManagement.Commands.RemoveLike
{
    public class RemoveLikeCommandValidator: AbstractValidator<RemoveLikeCommand>
    {
        public RemoveLikeCommandValidator()
        {
            RuleFor(x => x.PostId).NotEmpty();
        }
    }
}
