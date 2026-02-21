using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record UpdateCommunicationPreferencesCommand(
    string ProfileId,
    string UserId,
    bool? SmsNotifications,
    bool? EmailNotifications,
    bool? PushNotifications,
    EContactTime? PreferredContactTime,
    EmergencyContact? EmergencyContact);