using ActvShare.Domain.Common.Models;

namespace ActvShare.Domain.Chats.ValueObjects;

public class ChatId: AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }
    
    private ChatId(Guid value)
    {
        Value = value;
    }
    
    public static ChatId CreateUnique()
    {
        return new ChatId(Guid.NewGuid());
    }
    
    public static ChatId Create(Guid value)
    {
        return new(value);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    private ChatId(){}
}