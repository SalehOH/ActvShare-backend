using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.ChatManagement.Responses;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.Common.Responses;
using ActvShare.Domain.Chats;
using ActvShare.Domain.Chats.ValueObjects;
using ActvShare.Domain.Common.Errors;
using ActvShare.Domain.Users;
using ActvShare.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.ChatManagement.Queries.GetChat
{
    public class GetChatQueryHandler : IRequestHandler<GetChatQuery, ErrorOr<ChatResponse>>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;

        public GetChatQueryHandler(IChatRepository chatRepository, IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<ChatResponse>> Handle(GetChatQuery request, CancellationToken cancellationToken)
        {
            var chat = await _chatRepository.GetChatByIdAsync(ChatId.Create(request.ChatId), cancellationToken);
            if (chat is null)
                return Errors.Chat.ChatNotFound;

            User? currentUser, otherUser;

            if (chat.user1.Value == request.user)
            {
                currentUser = await _userRepository.GetUserByIdAsync(UserId.Create(chat.user1.Value), cancellationToken);
                otherUser = await _userRepository.GetUserByIdAsync(UserId.Create(chat.user2.Value), cancellationToken);
            }
            else
            {
                currentUser = await _userRepository.GetUserByIdAsync(UserId.Create(chat.user2.Value), cancellationToken);
                otherUser = await _userRepository.GetUserByIdAsync(UserId.Create(chat.user1.Value), cancellationToken);
            }


            ChatResponse response = new ChatResponse(new UserResponse(
                otherUser!.Name, otherUser.Username, otherUser.ProfileImage.StoredFileName),
                GetMessages(chat, currentUser!, otherUser)
            );


            return response;
        }

        private List<MessageResponse> GetMessages(Chat chat, User user1, User user2)
        {
            var messages = chat.Messages.Select(m =>
            {
                if (m.SenderId == user1!.Id)
                    return new MessageResponse(m.Id.Value, m.Content, user1.Username, m.SentAt);
                else
                    return new MessageResponse(m.Id.Value, m.Content, user2!.Username, m.SentAt);
            }).ToList();

            return messages;
        }
    }
}
