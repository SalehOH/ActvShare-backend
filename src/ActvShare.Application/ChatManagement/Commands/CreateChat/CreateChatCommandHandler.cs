using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Domain.Abstractions;
using ActvShare.Domain.Chats;
using ActvShare.Domain.Users.ValueObjects;
using MediatR;

namespace ActvShare.Application.ChatManagement.Commands.CreateChat
{
    public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, bool>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateChatCommandHandler(IChatRepository chatRepository, IUnitOfWork unitOfWork)
        {
            _chatRepository = chatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var chat = Chat.Create(UserId.Create(request.UserId), UserId.Create(request.OtherUserId));

            await _chatRepository.AddChatAsync(chat, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
