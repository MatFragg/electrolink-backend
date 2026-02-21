namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

public record MyProfileResource(
    string ProfileId,
    string UserId,
    string Status,
    string? BusinessRole,
    PersonalDataResource? PersonalData,
    TechnicianProfileResource? Technician,
    HomeownerProfileResource? Homeowner);