using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.ChatManagement.Commands.CreateMessage
{
    internal class CreateMessageValidato: AbstractValidator<CreateMessageCommand>
    {
        public CreateMessageValidato()
        {
            RuleFor(x => x.ChatId)
                .NotEmpty();

            RuleFor(x => x.SenderId)
                .NotEmpty();

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(250);
        }
    }
}
