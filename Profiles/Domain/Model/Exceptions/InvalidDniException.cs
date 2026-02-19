using System.Runtime.Serialization;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public class InvalidDniException : Exception
{
    public string RawValue { get; }

    public InvalidDniException(string rawValue)
        : base($"Invalid dni: '{rawValue}'")
    {
        RawValue = rawValue;
    }

    public InvalidDniException(string message, Exception inner) : base(message, inner) { }

    protected InvalidDniException(SerializationInfo info, StreamingContext context) : base(info, context)
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