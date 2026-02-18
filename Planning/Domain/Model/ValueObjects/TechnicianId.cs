namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record TechnicianId(Guid Id)
{
    public static TechnicianId From(Guid id) => new(id);
    
    public static TechnicianId From(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new ArgumentException($"Invalid TechnicianId format: {id}");
        return new TechnicianId(guid);
    }

    public override string ToString() => Id.ToString();
}