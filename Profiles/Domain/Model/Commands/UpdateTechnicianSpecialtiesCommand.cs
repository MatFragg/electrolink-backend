namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record UpdateTechnicianSpecialtiesCommand(string TechnicianId, IEnumerable<string> Specialties);