using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.Authentication.Queries.Login
{
    public class LoginQueryValidator: AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(5)
                .WithMessage("Invalid Credentials");
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("Invalid Credentials");
        }
    }
}
