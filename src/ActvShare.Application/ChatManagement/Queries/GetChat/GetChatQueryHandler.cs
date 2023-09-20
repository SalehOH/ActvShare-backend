using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.ChatManagement.Responses;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Chats.ValueObjects;
using ActvShare.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.ChatManagement.Queries.GetChat
{
    public class GetChatQueryHandler : IRequestHandler<GetChatQuery, ErrorOr<List<MessageResponse>>>
    {
        private readonly IChatRepository _chatRepository;

        public GetChatQueryHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<ErrorOr<List<MessageResponse>>> Handle(GetChatQuery request, CancellationToken cancellationToken)
        {
            var chat = await _chatRepository.GetChatByIdAsync(ChatId.Create(request.ChatId), cancellationToken);
            if (chat is null)
                return Errors.Chat.ChatNotFound;

            var messages = chat.Messages.Select(m => 
                new MessageResponse(m.Id.Value, m.Content, m.SenderId.Value, m.SentAt)).ToList();

            return messages;
        }
    }
}
