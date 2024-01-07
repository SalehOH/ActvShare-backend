using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(30)
            .WithMessage("Name can only contain 30 letters")
            .Matches("^[a-zA-Z ]*$")
            .WithMessage("Name can only contain 30 letters");

        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(25)
            .Matches("^[a-zA-Z0-9]*$")
            .WithMessage("Username can only contain letters and numbers");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$")
            .WithMessage("Password must contain at least 8 characters, one uppercase letter, one lowercase letter and one number")
            .DependentRules(() =>
            {
                RuleFor(x => x.ConfirmPassowrd)
                    .Equal(x => x.Password)
                    .WithMessage("Passwords do not match");
            });

        RuleFor(x => x.ProfileImage)
            .NotNull()
            .Must(x => x.ContentType.ToLower().Contains("image"))
            .WithMessage("Please upload a profile picture");



    }
}

