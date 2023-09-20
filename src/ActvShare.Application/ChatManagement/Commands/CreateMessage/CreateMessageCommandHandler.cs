using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.ChatManagement.Responses;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Chats.ValueObjects;
using ActvShare.Domain.Users.ValueObjects;
using ActvShare.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.ChatManagement.Commands.CreateMessage
{
    public class CreateMessageCommandHandler
        : IRequestHandler<CreateMessageCommand, ErrorOr<MessageResponse>>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUnitOfWork _unitOfWork;


        public CreateMessageCommandHandler(IChatRepository chatRepository, IUnitOfWork unitOfWork)
        {
            _chatRepository = chatRepository;
            _unitOfWork = unitOfWork;
        }
       
        public async Task<ErrorOr<MessageResponse>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {      
            var chat = await _chatRepository.GetChatByIdAsync(ChatId.Create(request.ChatId), cancellationToken);

            if (chat is null)
            {
                return Errors.Chat.ChatNotFound;
            }

            var message = chat.AddMessage(request.Content, UserId.Create(request.SenderId));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new MessageResponse(message.Id.Value, message.Content, request.SenderId, message.SentAt);
        }
    }
}
