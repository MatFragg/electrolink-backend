namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record RequestId(Guid Id)
{
    public static RequestId NewId() => new(Guid.NewGuid());
    
    public static RequestId From(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new ArgumentException($"Invalid RequestId format: {id}");
        return new RequestId(guid);
    }

    public override string ToString() => Id.ToString();
}