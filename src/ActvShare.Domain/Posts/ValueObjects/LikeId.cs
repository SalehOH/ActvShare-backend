using ActvShare.Domain.Common.Models;

namespace ActvShare.Domain.Posts.ValueObjects;

public class LikeId: ValueObject
{
    public Guid Value { get; }

    private LikeId(Guid value)
    {
        Value = value;
    }

    public static LikeId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static LikeId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }   
}