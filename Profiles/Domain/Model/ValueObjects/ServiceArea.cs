using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

using NetTopologySuite.Geometries;

public record ServiceArea
{
    public double CenterLatitude { get; init; }
    public double CenterLongitude { get; init; }
    public double RadiusKm { get; init; }

    public Geometry Area { get; init; }

    private static readonly GeometryFactory Factory =
        new(new PrecisionModel(), 4326);

    private ServiceArea() { }

    public static ServiceArea FromPointAndRadius(double lat, double lon, double radiusKm)
    {
        if (lat < -90 || lat > 90)
            throw new InvalidServiceAreaException("Latitud inválida");
        if (lon < -180 || lon > 180)
            throw new InvalidServiceAreaException("Longitud inválida");
        if (radiusKm <= 0 || radiusKm > 100)
            throw new InvalidServiceAreaException("El radio debe estar entre 0 y 100 km");

        var center = Factory.CreatePoint(new Coordinate(lon, lat));

        var radiusDegrees = radiusKm / 111.0;
        var area = center.Buffer(radiusDegrees, 32); 

        return new ServiceArea
        {
            CenterLatitude = lat,
            CenterLongitude = lon,
            RadiusKm = radiusKm,
            Area = area,
        };
    }

    public bool ContainsPoint(double lat, double lon)
    {
        var point = Factory.CreatePoint(new Coordinate(lon, lat));
        return Area.Contains(point);
    }

    internal static ServiceArea FromStored(double lat, double lon, double radiusKm, Geometry area)
        => new()
        {
            CenterLatitude = lat,
            CenterLongitude = lon,
            RadiusKm = radiusKm,
            Area = area,
        };
}