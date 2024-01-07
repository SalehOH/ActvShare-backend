using System.Security.Claims;
using ActvShare.Application.ChatManagement.Commands.CreateMessage;
using ActvShare.Application.ChatManagement.Queries.GetChat;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MediatR;

namespace ActvShare.Infrastructure.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ChatHub : Hub
{
    private readonly ISender _sender;

    public ChatHub(ISender sender)
    {
        _sender = sender;
    }

    public async Task SendMessage(string text)
    {
        var chatId = GetChatId();
        var senderId = GetUserId();
        var command = new CreateMessageCommand(chatId, senderId, text);

        var result = await _sender.Send(command);
        if (result.IsError)
        {
            return;
        }
        await Clients.Group($"{chatId}").SendAsync("ReceiveMessage", result.Value);
    }

    public override async Task OnConnectedAsync()
    {
        try
        {
            await JoinChat();
            await base.OnConnectedAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }

    private async Task JoinChat()
    {
        var chatId = GetChatId();
        var userId = GetUserId();
        var query = new GetChatQuery(chatId, userId);
        var result = await _sender.Send(query);

        if (result.IsError)
        {
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");
    }

    private Guid GetChatId()
    {
        var httpContext = Context.GetHttpContext();
        var chatId = httpContext?.Request.Query["chatId"];

        if (string.IsNullOrEmpty(chatId))
        {
            return Guid.Empty;
        }

        Guid guid;
        if (Guid.TryParse(chatId, out guid))
            return guid;


        return Guid.Empty;
    }

    private Guid GetUserId()
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Guid.Empty;
        }

        Guid guid;
        if (Guid.TryParse(userId, out guid))
            return guid;


        return Guid.Empty;
    }
}