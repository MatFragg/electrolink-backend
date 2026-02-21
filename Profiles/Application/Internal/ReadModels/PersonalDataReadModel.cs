namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.ReadModels;

public sealed record PersonalDataReadModel(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Address,
    string DateOfBirth
);