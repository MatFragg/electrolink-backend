namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

/// <summary>
/// Sub-record para el contacto de emergencia en requests de homeowner.
/// </summary>
public record EmergencyContactResource(
    string Name,
    string Relationship,
    string PhoneNumber);

