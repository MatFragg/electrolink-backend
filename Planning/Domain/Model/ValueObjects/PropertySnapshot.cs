namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record PropertySnapshot
{
    public Guid PropertyId { get; init; }
    public string Address { get; init; }
    public Geolocation Geolocation { get; init; }
    
    public PropertySnapshot() : this(Guid.Empty, string.Empty, new Geolocation()) { }
    
    public PropertySnapshot(Guid propertyId, string address, Geolocation geolocation)
    {
        PropertyId = propertyId;
        Address = address ?? string.Empty;
        Geolocation = geolocation ?? new Geolocation();
    }
}

