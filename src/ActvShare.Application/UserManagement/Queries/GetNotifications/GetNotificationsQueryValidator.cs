using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.UserManagement.Queries.GetNotifications
{
    public class GetSearchUserQueryValidator: AbstractValidator<GetNotificationQuery>
    {
        public GetSearchUserQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
