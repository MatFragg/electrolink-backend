using System.Runtime.Serialization;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public class UnderageUserException : Exception
{
    public int Age { get; }

    public UnderageUserException(int age)
        : base($"User is underage: {age}")
    {
        Age = age;
    }

    public UnderageUserException(string message) : base(message) { }

    public UnderageUserException(string message, Exception inner) : base(message, inner) { }

    protected UnderageUserException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Age = info.GetInt32(nameof(Age));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null) throw new ArgumentNullException(nameof(info));
        info.AddValue(nameof(Age), Age);
        base.GetObjectData(info, context);
    }
}