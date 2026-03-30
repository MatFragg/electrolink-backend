using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record PropertySnapshot
{
    public string PropertyId { get; init; }
    public string Address { get; init; }
    public Geolocation Geolocation { get; init; }
    
    public PropertySnapshot() : this(string.Empty, string.Empty, Geolocation.Create(0, 0, null, "MANUAL")) { }
    
    public PropertySnapshot(string propertyId, string address, Geolocation geolocation)
    {
        PropertyId = propertyId;
        Address = address ?? string.Empty;
        Geolocation = geolocation;
    }
}
