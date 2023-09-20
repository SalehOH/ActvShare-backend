using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.Common.Interfaces.Persistance;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Users.ValueObjects;
using ActvShare.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Queries.GetNotifications
{
    public class GetNotificationQueryHandler : IRequestHandler<GetNotificationQuery, ErrorOr<List<NotificationResponse>>>
    {
        private readonly IUserRepository _userRepository;

        public GetNotificationQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<List<NotificationResponse>>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(UserId.Create(request.UserId), cancellationToken);
            if (user is null)
            {
                return Errors.User.UserNotFound;
            }
            
            if (user.Notifications is null || user.Notifications.Any() is not true)
                return Errors.User.NoNotificationsFound;
            
            
            var notifications = user.Notifications
                .Select(n => 
                    new NotificationResponse(n.Id.Value, n.Message, n.IsRead, n.CreatedAt))
                .ToList();

            return notifications;
        }
    }
}
