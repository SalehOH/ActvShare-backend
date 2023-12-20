using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Queries.GetSearchUser
{
    public class GetSearchUserQueryHandler : IRequestHandler<GetSearchUserQuery, ErrorOr<List<SearchUserResponse>>>
    {
        private readonly IUserRepository _userRepository;

        public GetSearchUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<List<SearchUserResponse>>> Handle(GetSearchUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetSearchedUserAsync(request.SerachUsername, cancellationToken);

            if (user.Any() is not true)
            {
                return Errors.User.UserNotFound;
            }

            return user.Select(u => 
                new SearchUserResponse(u.Name, u.Username, u.ProfileImage.StoredFileName)
            ).ToList();
        }
    }
}