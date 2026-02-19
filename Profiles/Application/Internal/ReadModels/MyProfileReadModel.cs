namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ReadModels;


public sealed record MyProfileReadModel(
    Guid ProfileId,
    Guid UserId,
    string? BusinessRole,
    string Status,
    PersonalDataReadModel? PersonalData,
    TechnicianReadModel? Technician,
    HomeownerReadModel? Homeowner
);
