using FluentValidation;

namespace ActvShare.Application.UserManagement.Queries.GetUserPosts;
public class GetUserPostsQueryValidator : AbstractValidator<GetUserPostsQuery>
{
    public GetUserPostsQueryValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username must be provided.");
    }
}
