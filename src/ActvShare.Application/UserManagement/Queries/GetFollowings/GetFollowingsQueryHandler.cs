using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.UserManagement.Queries.GetFollowers;
using ActvShare.Application.UserManagement.Responses;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Queries.GetFollowings
{
    public class GetFollowingsQueryHandler : IRequestHandler<GetFollowingsQuery, ErrorOr<List<FollowResponse>>>
    {
        private readonly IUserRepository _userRepository;

        public GetFollowingsQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<List<FollowResponse>>> Handle(GetFollowingsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.Username, cancellationToken);

            if (user is null)
                return Error.NotFound("User not found");

            var followings = user.Follows;

            if (followings is null || !followings.Any())
            {
                return Error.NotFound("Followings not found");
            }

            var followResponses = await Task.WhenAll(followings.Select(async following =>
            {
                var followingUser = await _userRepository.GetUserByIdAsync(following.FollowedUserId, cancellationToken);
                return new FollowResponse(followingUser!.Name, followingUser.Username, followingUser.ProfileImage.StoredFileName);
            }));

            

            return followResponses.ToList();
        }
    }
}
