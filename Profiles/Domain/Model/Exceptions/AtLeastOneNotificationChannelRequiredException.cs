using System.Runtime.Serialization;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public sealed class AtLeastOneNotificationChannelRequiredException : Exception
{
    public AtLeastOneNotificationChannelRequiredException()
        : base("At least one notification channel must be active (sms, email or push).")
    {
    }

    public AtLeastOneNotificationChannelRequiredException(string message)
        : base(message)
    {
    }

    public AtLeastOneNotificationChannelRequiredException(string message, Exception inner)
        : base(message, inner)
    {
    }

    protected AtLeastOneNotificationChannelRequiredException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null) throw new ArgumentNullException(nameof(info));
        base.GetObjectData(info, context);
    }
}