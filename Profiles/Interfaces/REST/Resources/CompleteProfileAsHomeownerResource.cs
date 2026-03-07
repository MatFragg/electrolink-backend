using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

/// <summary>
/// Resource de request para POST /me/complete/homeowner.
/// </summary>
public record CompleteProfileAsHomeownerResource(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Dni,
    string DateOfBirth,
    string Street,
    string District,
    string City,
    string Country,
    string PostalCode,
    EContactTime PreferredContactTime,
    bool SmsNotifications,
    bool EmailNotifications,
    bool PushNotifications,
    EmergencyContactResource? EmergencyContact = null);

