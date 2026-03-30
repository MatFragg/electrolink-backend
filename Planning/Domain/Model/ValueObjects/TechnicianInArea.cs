namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record TechnicianInArea(string TechnicianId, string ProfileId, string FullName, IReadOnlyList<string> Specialties, double Rating);
