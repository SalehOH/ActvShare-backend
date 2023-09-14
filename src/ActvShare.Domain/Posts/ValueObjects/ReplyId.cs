using ActvShare.Domain.Common.Models;

namespace ActvShare.Domain.Posts.ValueObjects;

public class ReplyId: ValueObject
{
    public Guid Value { get; }

    private ReplyId(Guid value)
    {
        Value = value;
    }

    public static ReplyId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static ReplyId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }   
    
}