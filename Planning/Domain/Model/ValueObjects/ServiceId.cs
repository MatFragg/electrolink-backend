namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ServiceId(Guid Id)
{
    public static ServiceId NewId() => new(Guid.NewGuid());
    
    public static ServiceId From(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new ArgumentException($"Invalid ServiceId format: {id}");
        return new ServiceId(guid);
    }

    public override string ToString() => Id.ToString();
}