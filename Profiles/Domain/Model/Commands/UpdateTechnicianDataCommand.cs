using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record UpdateTechnicianDataCommand(
    string ProfileId,
    string UserId,
    IEnumerable<ESpecialty>? Specialties,
    int? ExperienceYears,
    string? AboutMe);