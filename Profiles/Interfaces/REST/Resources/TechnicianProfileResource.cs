namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

public record TechnicianProfileResource(
    string TechnicianId,
    List<string> Specialties,
    int ExperienceYears,
    string AboutMe);