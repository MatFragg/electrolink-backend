using System.Runtime.Serialization;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public sealed class CertificationNotFoundException : Exception
{
    public CertificationId? CertificationId { get; }

    public CertificationNotFoundException()
        : base("Certification not found.")
    {
    }

    public CertificationNotFoundException(CertificationId certificationId)
        : base($"Certification with id '{certificationId}' was not found.")
    {
        CertificationId = certificationId;
    }

    public CertificationNotFoundException(string message)
        : base(message)
    {
    }

    public CertificationNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    private CertificationNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        CertificationId = (CertificationId?)info.GetValue(nameof(CertificationId), typeof(CertificationId));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(CertificationId), CertificationId, typeof(CertificationId));
    }
}