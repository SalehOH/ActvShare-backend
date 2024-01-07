using Microsoft.AspNetCore.Mvc;
using MediatR;
using ActvShare.Application.ChatManagement.Commands.CreateChat;
using ActvShare.Application.ChatManagement.Queries.GetAllChats;
using ActvShare.Application.ChatManagement.Queries.GetChat;

namespace ActvShare.WebApi.Controllers
{
    [ApiController]
    [Route("chat")]
    public class ChatManagementController : BaseController
    {
        private readonly ISender _sender;

        public ChatManagementController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("{otherUser}")]
        public async Task<IActionResult> CreateChat(string otherUser, CancellationToken cancellationToken)
        {
            var userId = GetAuthenticatedUserId();
            var command = new CreateChatCommand(userId, otherUser);
            
            var result = await _sender.Send(command, cancellationToken);
            return result.Match(
                success => Ok(success),
                error => Problem(error));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChats(CancellationToken cancellationToken)
        {
            var userId = GetAuthenticatedUserId();
            var query = new GetAllChatsQuery(userId);
            
            var result = await _sender.Send(query, cancellationToken);
            return result.Match(
                success => Ok(success),
                error => Problem(error));
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChat(Guid chatId, CancellationToken cancellationToken)
        {
            var userId = GetAuthenticatedUserId();
            var query = new GetChatQuery(chatId, userId);
            
            var result = await _sender.Send(query, cancellationToken);
            return result.Match(
                success => Ok(success),
                error => Problem(error));
        }

    }
}