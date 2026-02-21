using System.Runtime.Serialization;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public class InvalidBusinessRoleException : Exception
{
    public string ProfileId { get; }
    public string Expected { get; }
    public string Actual { get; }

    public InvalidBusinessRoleException()
    { }

    public InvalidBusinessRoleException(ProfileId? profileId, EBusinessRole expected, EBusinessRole? actual)
        : base(profileId is null
            ? $"Invalid business role. Expected '{expected}', actual '{actual?.ToString() ?? "null"}'."
            : $"Profile '{profileId}' has invalid business role. Expected '{expected}', actual '{actual?.ToString() ?? "null"}'.")
    {
        ProfileId = profileId?.ToString() ?? string.Empty;
        Expected = expected.ToString();
        Actual = actual?.ToString() ?? string.Empty;
    }

    public InvalidBusinessRoleException(string? message, Exception? innerException)
        : base(message, innerException)
    { }

    protected InvalidBusinessRoleException(SerializationInfo info, StreamingContext context)
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