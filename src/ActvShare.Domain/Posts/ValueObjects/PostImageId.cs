using ActvShare.Domain.Common.Models;

namespace ActvShare.Domain.Posts.ValueObjects;

public class PostImageId: ValueObject
{
    public Guid Value { get; }

    private PostImageId(Guid value)
    {
        Value = value;
    }

    public static PostImageId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static PostImageId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }   
}