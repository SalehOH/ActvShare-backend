using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ActvShare.Application.PostManagement.Queries.GetPosts
{
    public class GetPostsQueryValidator: AbstractValidator<GetPostsQuery>
    {
        public GetPostsQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
