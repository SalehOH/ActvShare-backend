using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.PostManagement.Commands.LikePost
{
    public class RemoveLikePostCommandValidator: AbstractValidator<LikePostCommand>
    {
        public RemoveLikePostCommandValidator()
        {
            RuleFor(x => x.PostId).NotEmpty();
        }
    }
}
