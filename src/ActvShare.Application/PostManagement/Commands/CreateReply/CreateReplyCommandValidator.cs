using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.PostManagement.Commands.CreateReply
{
    public class CreateReplyCommandValidator: AbstractValidator<CreateReplyCommand>
    {
        public CreateReplyCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content must be provided");
        }
    }
}
