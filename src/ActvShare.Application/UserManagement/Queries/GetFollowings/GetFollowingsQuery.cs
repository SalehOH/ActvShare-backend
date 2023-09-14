using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActvShare.Application.UserManagement.Responses;
using ActvShare.Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;

namespace ActvShare.Application.UserManagement.Queries.GetFollowers
{
    public record GetFollowingsQuery(string Username) : IRequest<ErrorOr<List<FollowResponse>>>;
}