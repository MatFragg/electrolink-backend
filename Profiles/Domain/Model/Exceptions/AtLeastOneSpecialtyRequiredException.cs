// ...existing code...
using System.Runtime.Serialization;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public sealed class AtLeastOneSpecialtyRequiredException : Exception
{
    public AtLeastOneSpecialtyRequiredException()
        : base("At least one specialty is required.")
    {
    }

    public AtLeastOneSpecialtyRequiredException(string message)
        : base(message)
    {
    }

    public AtLeastOneSpecialtyRequiredException(string message, Exception inner)
        : base(message, inner)
    {
    }

    protected AtLeastOneSpecialtyRequiredException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null) throw new ArgumentNullException(nameof(info));
        base.GetObjectData(info, context);
    }
}
