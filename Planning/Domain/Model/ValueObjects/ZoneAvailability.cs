namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ZoneAvailability(
    string TechnicianId,
    string District,
    string Region
);