using System.Runtime.Serialization;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public sealed class UnauthorizedProfileAccessException : UnauthorizedAccessException
{
    public ProfileId? ProfileId { get; }
    public string? UserId { get; }

    public UnauthorizedProfileAccessException()
        : base("Unauthorized access to profile.")
    {
    }

    public UnauthorizedProfileAccessException(string message)
        : base(message)
    {
    }

    public UnauthorizedProfileAccessException(ProfileId profileId, string userId)
        : base($"User '{userId}' is not authorized to access profile '{profileId}'.")
    {
        ProfileId = profileId;
        UserId = userId;
    }

    public UnauthorizedProfileAccessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    private UnauthorizedProfileAccessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        // Optional: deserialize custom properties if needed
    }
}