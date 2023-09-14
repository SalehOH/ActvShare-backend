using ActvShare.Domain.Common.Models;

namespace ActvShare.Domain.Posts.ValueObjects;

public class PostId: AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }
    
    private PostId(Guid value)
    {
        Value = value;
    }
    
    public static PostId CreateUnique()
    {
        return new PostId(Guid.NewGuid());
    }
    
    public static PostId Create(Guid value)
    {
        return new(value);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    private PostId(){}
    
}