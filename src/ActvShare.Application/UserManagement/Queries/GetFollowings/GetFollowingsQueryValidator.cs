using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.UserManagement.Queries.GetFollowings
{
    public class GetNotificationsQueryValidator: AbstractValidator<GetFollowingsQuery>
    {
        public GetNotificationsQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
