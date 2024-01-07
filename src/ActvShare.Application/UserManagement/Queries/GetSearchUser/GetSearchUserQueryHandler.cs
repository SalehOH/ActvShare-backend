using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Users;
using ActvShare.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Queries.GetSearchUser
{
    public class GetSearchUserQueryHandler : IRequestHandler<GetSearchUserQuery, ErrorOr<List<SearchUserResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;

        public GetSearchUserQueryHandler(IUserRepository userRepository, IChatRepository chatRepository)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
        }

        public async Task<ErrorOr<List<SearchUserResponse>>> Handle(GetSearchUserQuery request, CancellationToken cancellationToken)
        {

            var searchResults = await _userRepository.GetSearchedUserAsync(request.SerachUsername.ToLower(), cancellationToken);

            if (searchResults.Any() is not true)
            {
                return Errors.User.UserNotFound;
            }

            User? user = null;
            if (request.userId is not null)
            {
                user = await _userRepository.GetUserByIdAsync(UserId.Create(request.userId.Value), cancellationToken);
                searchResults = searchResults.Where(u => u.Id.Value != request.userId).ToList();
            }
            

            var tasks = searchResults.Select(async u =>
            {
                bool? isFollowed = null;
                bool? hasChat = null;

                if (user is not null)
                {
                    isFollowed = user.Follows.Any(f => f.FollowedUserId == u.Id);
                    hasChat = await _chatRepository.ChatExistsAsync(UserId.Create(u.Id.Value), UserId.Create(user.Id.Value), cancellationToken);
                }
                return new SearchUserResponse(u.Name, u.Username, u.ProfileImage.StoredFileName, isFollowed, hasChat);
            });

            var result = await Task.WhenAll(tasks);

            return result.ToList();
        }
    }
}