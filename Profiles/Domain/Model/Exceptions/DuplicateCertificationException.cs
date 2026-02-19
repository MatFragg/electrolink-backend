using System.Runtime.Serialization;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public sealed class DuplicateCertificationException : Exception
{
    public object? CredentialId { get; }

    public DuplicateCertificationException()
        : base("A duplicate certification was detected.")
    {
    }

    public DuplicateCertificationException(object credentialId)
        : base($"A duplicate certification was detected for credential '{credentialId}'.")
    {
        CredentialId = credentialId;
    }

    public DuplicateCertificationException(string message)
        : base(message)
    {
    }

    public DuplicateCertificationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected DuplicateCertificationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        CredentialId = info.GetValue(nameof(CredentialId), typeof(object));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(CredentialId), CredentialId, typeof(object));
    }
}