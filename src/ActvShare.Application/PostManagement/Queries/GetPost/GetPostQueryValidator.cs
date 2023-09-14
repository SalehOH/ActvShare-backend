using FluentValidation;

namespace ActvShare.Application.PostManagement.Queries.GetPost
{
    public class GetPostQueryValidator: AbstractValidator<GetPostQuery>
    {
        public GetPostQueryValidator()
        {
            RuleFor(x => x.PostId).NotEmpty();
        }
    }
}
