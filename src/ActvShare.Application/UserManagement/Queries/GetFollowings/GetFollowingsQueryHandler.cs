using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Users.ValueObjects;
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
            var user = await _userRepository.GetUserByIdAsync(UserId.Create(request.UserId), cancellationToken);

            if (user is null)
                return Errors.User.UserNotFound;

            var followings = user.Follows;

            if (followings is null || followings.Any() is not true)
            {
                return Errors.User.NoFollowersFound;
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
