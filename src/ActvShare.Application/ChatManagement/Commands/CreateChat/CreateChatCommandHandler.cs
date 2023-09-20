using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Chats;
using ActvShare.Domain.Users.ValueObjects;
using ActvShare.Domain.Common.Errors;
using MediatR;
using ErrorOr;

namespace ActvShare.Application.ChatManagement.Commands.CreateChat
{
    public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, ErrorOr<bool>>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateChatCommandHandler(IChatRepository chatRepository, IUnitOfWork unitOfWork)
        {
            _chatRepository = chatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<bool>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var chatExists = await _chatRepository.ChatExistsAsync(UserId.Create(request.UserId), UserId.Create(request.OtherUserId), cancellationToken);
            if (chatExists is not true)
            {
                return Errors.Chat.ChatAlreadyExists;
            }
            
            var chat = Chat.Create(UserId.Create(request.UserId), UserId.Create(request.OtherUserId));

            await _chatRepository.AddChatAsync(chat, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
