using System.Runtime.Serialization;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public class ProfileAlreadyExistsException : Exception
{
    public string UserId { get; }

    public ProfileAlreadyExistsException()
    { }

    public ProfileAlreadyExistsException(string? userId)
        : base(userId is null ? "A profile already exists." : $"A profile for user '{userId}' already exists.")
    {
        UserId = userId ?? string.Empty;
    }

    public ProfileAlreadyExistsException(string? message, Exception? innerException)
        : base(message, innerException)
    { }

    protected ProfileAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        UserId = info.GetString(nameof(UserId)) ?? string.Empty;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info is null) throw new ArgumentNullException(nameof(info));
        info.AddValue(nameof(UserId), UserId);
        base.GetObjectData(info, context);
    }
}
    
