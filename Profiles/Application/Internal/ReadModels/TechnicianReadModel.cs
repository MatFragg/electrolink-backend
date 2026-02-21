namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.ReadModels;

public sealed record TechnicianReadModel(
    string TechnicianId,
    IReadOnlyList<string> Specialties,
    int ExperienceYears,
    string? AboutMe
);