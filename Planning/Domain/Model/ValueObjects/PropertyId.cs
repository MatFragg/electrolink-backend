namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record PropertyId(Guid Id)
{
    public static PropertyId From(Guid id) => new(id);
    
    public static PropertyId From(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new ArgumentException($"Invalid PropertyId format: {id}");
        return new PropertyId(guid);
    }

    public override string ToString() => Id.ToString();
}