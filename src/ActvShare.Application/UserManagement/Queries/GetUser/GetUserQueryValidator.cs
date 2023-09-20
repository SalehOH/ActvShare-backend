using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.UserManagement.Queries.GetUser
{
    public class GetUserQueryValidator: AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username must be provided.");
        }
    }
}
