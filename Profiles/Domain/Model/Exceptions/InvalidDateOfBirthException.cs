using System.Runtime.Serialization;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public class InvalidDateOfBirthException : Exception
{
    public string RawValue { get; }

    public InvalidDateOfBirthException(string rawValue)
        : base($"Invalid date of birth: '{rawValue}'")
    {
        RawValue = rawValue;
    }

    public InvalidDateOfBirthException(string message, Exception inner) : base(message, inner) { }

    protected InvalidDateOfBirthException(SerializationInfo info, StreamingContext context) : base(info, context)
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