namespace ActvShare.Application.UserManagement.Responses
{
    public record NotificationResponse(Guid Id, string Message,bool IsRead, DateTime CreatedAt);
}
