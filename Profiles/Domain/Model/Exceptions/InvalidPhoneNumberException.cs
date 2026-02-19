using System.Runtime.Serialization;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public class InvalidPhoneNumberException : Exception
{
    public string RawValue { get; }

    public InvalidPhoneNumberException(string rawValue)
        : base($"Invalid phone number: '{rawValue}'")
    {
        RawValue = rawValue;
    }
    public InvalidPhoneNumberException(string message, Exception inner) : base(message, inner) { }

    protected InvalidPhoneNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        RawValue = info.GetString(nameof(RawValue));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null) throw new ArgumentNullException(nameof(info));
        info.AddValue(nameof(RawValue), RawValue);
        base.GetObjectData(info, context);
    }
}