using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

public record CommunicationPreferences
{
    public bool        SmsNotifications   { get; init; }
    public bool        EmailNotifications { get; init; }
    public bool        PushNotifications  { get; init; }
    public EContactTime PreferredContactTime { get; init; }

    private CommunicationPreferences() { }

    public static CommunicationPreferences Create(bool sms, bool email, bool push, EContactTime? time = null)
    {
        var prefs = new CommunicationPreferences
        {
            SmsNotifications    = sms,
            EmailNotifications  = email,
            PushNotifications   = push,
            PreferredContactTime = time ?? EContactTime.Morning,
        };

        if (!prefs.HasAtLeastOneChannelActive())
            throw new AtLeastOneNotificationChannelRequiredException();

        return prefs;
    }

    public bool HasAtLeastOneChannelActive()
        => SmsNotifications || EmailNotifications || PushNotifications;
}