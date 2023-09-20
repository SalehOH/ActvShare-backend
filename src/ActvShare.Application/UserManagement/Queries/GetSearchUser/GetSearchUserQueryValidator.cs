using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.UserManagement.Queries.GetSearchUser
{
    public class GetUserQueryValidator: AbstractValidator<GetSearchUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(x => x.SerachUsername)
                .NotEmpty().WithMessage("Username must be provided.");
        }
    }
}
