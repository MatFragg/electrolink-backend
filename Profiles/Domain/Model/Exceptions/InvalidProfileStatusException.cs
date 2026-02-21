using System.Runtime.Serialization;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public class InvalidProfileStatusException : Exception
{
    public string ProfileId { get; }
    public string Expected { get; }
    public string Actual { get; }

    public InvalidProfileStatusException()
    { }

    public InvalidProfileStatusException(ProfileId? profileId, EProfileStatus expected, EProfileStatus actual)
        : base(profileId is null
            ? $"Invalid profile status. Expected '{expected}', actual '{actual}'."
            : $"Profile '{profileId}' has invalid status. Expected '{expected}', actual '{actual}'.")
    {
        ProfileId = profileId?.ToString() ?? string.Empty;
        Expected = expected.ToString();
        Actual = actual.ToString();
    }

    public InvalidProfileStatusException(string? message, Exception? innerException)
        : base(message, innerException)
    { }

    protected InvalidProfileStatusException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ProfileId = info.GetString(nameof(ProfileId)) ?? string.Empty;
        Expected = info.GetString(nameof(Expected)) ?? string.Empty;
        Actual = info.GetString(nameof(Actual)) ?? string.Empty;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info is null) throw new ArgumentNullException(nameof(info));
        info.AddValue(nameof(ProfileId), ProfileId);
        info.AddValue(nameof(Expected), Expected);
        info.AddValue(nameof(Actual), Actual);
        base.GetObjectData(info, context);
    }
}