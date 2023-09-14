using ActvShare.Domain.Common.Models;

namespace ActvShare.Domain.Users.ValueObjects
{
    public class NotificationId: ValueObject
    {
        public Guid Value { get; }

        private NotificationId(Guid value)
        {
            Value = value;
        }

        public static NotificationId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static NotificationId Create(Guid value)
        {
            return new(value);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
        
        private NotificationId() { }
    }
}