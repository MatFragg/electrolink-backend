namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record UpdateProfilePersonalDataCommand( 
    string ProfileId,
    string UserId,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Street,
    string? Number,
    string? District,
    string? City,
    string? Country,
    string? PostalCode);