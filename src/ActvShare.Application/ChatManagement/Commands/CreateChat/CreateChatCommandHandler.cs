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
        private readonly IUserRepository _userRepository;

        public CreateChatCommandHandler(IChatRepository chatRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<bool>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var otherUser = await _userRepository.GetUserByUsernameAsync(request.OtherUser, cancellationToken);
            if (otherUser is null)
            {
                return Errors.User.UserNotFound;
            }
            
            var chatExists = await _chatRepository.ChatExistsAsync(
                UserId.Create(request.UserId), UserId.Create(otherUser.Id.Value), cancellationToken
            );
            
            if (chatExists)
            {
                return Errors.Chat.ChatAlreadyExists;
            }
            var chat = Chat.Create(UserId.Create(request.UserId), UserId.Create(otherUser.Id.Value));

            await _chatRepository.AddChatAsync(chat, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
