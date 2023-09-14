using ActvShare.Domain.Common.Models;

namespace ActvShare.Domain.Users.ValueObjects;

public class UserId: AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }
    
    private UserId(Guid value)
    {
        Value = value;
    }
    
    public static UserId CreateUnique()
    {
        return new UserId(Guid.NewGuid());
    }
    
    public static UserId Create(Guid value)
    {
        return new(value);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    private UserId(){}   
}