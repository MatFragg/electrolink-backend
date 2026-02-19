using System.Runtime.Serialization;
namespace Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

[Serializable]
public class InvalidIdException : Exception
{
    public string IdType { get; }
    public string InvalidValue { get; }

    public InvalidIdException(string idType, string invalidValue)
        : base($"Invalid {idType} id: '{invalidValue}'")
    {
        IdType = idType;
        InvalidValue = invalidValue;
    }

    public InvalidIdException(string message) : base(message) { }

    public InvalidIdException(string message, Exception inner) : base(message, inner) { }

    protected InvalidIdException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        IdType = info.GetString(nameof(IdType));
        InvalidValue = info.GetString(nameof(InvalidValue));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null) throw new ArgumentNullException(nameof(info));
        info.AddValue(nameof(IdType), IdType);
        info.AddValue(nameof(InvalidValue), InvalidValue);
        base.GetObjectData(info, context);
    }
}