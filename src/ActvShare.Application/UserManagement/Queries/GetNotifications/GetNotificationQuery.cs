using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.UserManagement.Responses;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Queries.GetNotifications
{
    public sealed record GetNotificationQuery(Guid UserId): IRequest<ErrorOr<List<NotificationResponse>>> ;
}
