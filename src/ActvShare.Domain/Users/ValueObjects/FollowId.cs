using ActvShare.Domain.Common.Models;

namespace ActvShare.Domain.Users.ValueObjects;

public class FollowId: ValueObject
{
    public Guid Value { get; }

    private FollowId(Guid value)
    {
        Value = value;
    }

    public static FollowId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static FollowId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }  
    private FollowId(){}
}