namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record Geolocation
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    
    public Geolocation() : this(0, 0) { }
    
    public Geolocation(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Invalid latitude");
        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Invalid longitude");
            
        Latitude = latitude;
        Longitude = longitude;
    }
}

