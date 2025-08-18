namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record UpdateNotificationSettingsCommand(
    int ProfileId,
    bool EmailNotificationsEnabled,
    bool PushNotificationsEnabled
);