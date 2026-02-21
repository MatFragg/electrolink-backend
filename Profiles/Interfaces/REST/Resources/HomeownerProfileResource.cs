namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

public record HomeownerProfileResource(
    string HomeownerId,
    string PreferredContactTime,
    bool SmsNotifications,
    bool EmailNotifications,
    bool PushNotifications,
    string? EmergencyContactName,
    string? EmergencyContactRelationship,
    string? EmergencyContactPhone);