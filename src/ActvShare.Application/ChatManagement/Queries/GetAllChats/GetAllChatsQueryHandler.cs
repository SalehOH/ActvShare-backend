using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.ChatManagement.Responses;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.Common.Responses;
using ActvShare.Domain.Users.ValueObjects;
using ActvShare.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.ChatManagement.Queries.GetAllChats
{
    public class GetAllChatsQueryHandler : IRequestHandler<GetAllChatsQuery, ErrorOr<List<ViewChatResponse>>>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;

        public GetAllChatsQueryHandler(IChatRepository chatRepository, IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<List<ViewChatResponse>>> Handle(GetAllChatsQuery request, CancellationToken cancellationToken)
        {
            var chats = await _chatRepository.GetAllChatsAsync(UserId.Create(request.UserId), cancellationToken);
            
            if (chats.Any() is not true)
                return Errors.Chat.ChatNotFound;

            var viewChats = await Task.WhenAll(chats.Select(async chat =>
            {
                var otherUserId = chat.user1.Value == request.UserId ? chat.user2 : chat.user1;
                var user = await _userRepository.GetUserByIdAsync(otherUserId);

                return new ViewChatResponse
                (
                    chat.Id.Value,
                    new UserResponse(user!.Name, user.Username, user.ProfileImage.StoredFileName),
                    chat.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault()?.Content,
                    chat.UpdatedAt
                );
            }));


            return viewChats.ToList();
        }
    }

}
