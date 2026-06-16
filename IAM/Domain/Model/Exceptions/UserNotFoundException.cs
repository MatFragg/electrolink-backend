using System.Runtime.Serialization;

namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Exceptions;

[Serializable]
public class UserNotFoundException : Exception
{
    public string? UserId { get; }

    public UserNotFoundException() { }

    public UserNotFoundException(string? message) : base(message) { }

    public UserNotFoundException(string? message, string? userId) : base(message)
    {
        UserId = userId;
    }

    public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

    protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        UserId = info.GetString(nameof(UserId));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info is null) throw new ArgumentNullException(nameof(info));
        info.AddValue(nameof(UserId), UserId);
        base.GetObjectData(info, context);
    }
}
