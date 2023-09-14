using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.PostManagement.Commands.CreatePost
{
    public class CreatePostCommandValidator: AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .When(x => x.ContentPicture == null)
                .WithMessage("Content or Picture must be provided");

            RuleFor(x => x.ContentPicture)
                .NotEmpty()
                .When(x => string.IsNullOrEmpty(x.Content))
                .WithMessage("Content or Picture must be provided");
        }
    }
}
