namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

public record TechnicianDataResource(
    List<string> Specialties,
    int ExperienceYears,
    string AboutMe
);