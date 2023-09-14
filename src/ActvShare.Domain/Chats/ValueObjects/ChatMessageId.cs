using ActvShare.Domain.Common.Models;

namespace ActvShare.Domain.Chats.ValueObjects;

public class ChatMessageId: ValueObject
{
    public Guid Value { get; }

    private ChatMessageId(Guid value)
    {
        Value = value;
    }

    public static ChatMessageId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static ChatMessageId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }   
}