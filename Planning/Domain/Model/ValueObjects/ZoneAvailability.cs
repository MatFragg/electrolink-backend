namespace Hampcoders.Electrolink.API.Planning.API.Domain.Model.ValueObjects;

public record ZoneAvailability(
    string TechnicianId,
    string District,
    string Region
);