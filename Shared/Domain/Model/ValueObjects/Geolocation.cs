namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record Geolocation
{
    public decimal Latitude  { get; init; }
    public decimal Longitude { get; init; }
    public int? Accuracy  { get; init; }  
    public string Source { get; init; }  

    private Geolocation() { Source = "MANUAL"; }

    public static Geolocation Create(decimal lat, decimal lon, int? accuracy, string source)
    {
        if (lat < -90 || lat > 90)
            throw new ArgumentOutOfRangeException(nameof(lat), "Latitude must be between -90 and 90.");
        if (lon < -180 || lon > 180)
            throw new ArgumentOutOfRangeException(nameof(lon), "Longitude must be between -180 and 180.");
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException("Source is required.");

        return new Geolocation { Latitude = lat, Longitude = lon, Accuracy = accuracy, Source = source };
    }
}