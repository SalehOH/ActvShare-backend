using ActvShare.Domain.Common.Models;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Domain.Users.Entities
{
    public class Notification: Entity<NotificationId>
    {
        public string Message { get; }
        public bool IsRead { get; }
        public DateTime CreatedAt { get; }

        private Notification(NotificationId id, string message) : base(id)
        {
           Message = message;
           CreatedAt = DateTime.UtcNow;
           IsRead = false;
        }
        public static Notification Create(string message)
        {
            return new(NotificationId.CreateUnique(), message);
        }
        private Notification() { }
    }
}
