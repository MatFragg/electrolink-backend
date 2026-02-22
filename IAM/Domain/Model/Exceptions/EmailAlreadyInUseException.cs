using System.Runtime.Serialization;
using Hampcoders.Electrolink.API.IAM.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Exceptions;

[Serializable]
public class EmailAlreadyInUseException : Exception
{
    public string Email { get; }

    public EmailAlreadyInUseException()
    { }

    public EmailAlreadyInUseException(Email? email)
        : base(email is null ? "An email is already in use." : $"The email '{email}' is already in use.")
    {
        Email = email?.ToString() ?? string.Empty;
    }

    public EmailAlreadyInUseException(string? message, Exception? innerException)
        : base(message, innerException)
    { }

    protected EmailAlreadyInUseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        Email = info.GetString(nameof(Email)) ?? string.Empty;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info is null) throw new ArgumentNullException(nameof(info));
        info.AddValue(nameof(Email), Email);
        base.GetObjectData(info, context);
    }
}