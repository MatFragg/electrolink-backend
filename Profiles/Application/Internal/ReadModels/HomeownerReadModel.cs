namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.ReadModels;

public sealed record HomeownerReadModel(
    string HomeownerId,
    string PreferredContactTime,
    bool SmsNotifications,
    bool EmailNotifications,
    bool PushNotifications
);