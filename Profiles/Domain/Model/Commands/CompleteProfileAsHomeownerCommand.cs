using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record CompleteProfileAsHomeownerCommand(
    string UserId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Dni,
    string DateOfBirth,
    string Street,
    string Number,
    string District,
    string City,
    string Country,
    string PostalCode,
    EContactTime PreferredContactTime,
    CommunicationPreferences CommunicationPreferences,
    EmergencyContact? EmergencyContact = null);