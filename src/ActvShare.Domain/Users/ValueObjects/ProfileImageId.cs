using ActvShare.Domain.Common.Models;
namespace ActvShare.Domain.Users.ValueObjects;

public class ProfileImageId: ValueObject
{
    public Guid Value { get; }

    private ProfileImageId(Guid value)
    {
        Value = value;
    }

    public static ProfileImageId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static ProfileImageId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    private ProfileImageId(){}
}