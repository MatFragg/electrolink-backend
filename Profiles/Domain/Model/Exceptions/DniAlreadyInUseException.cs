using System.Runtime.Serialization;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public sealed class DniAlreadyInUseException : Exception
{
    public string Dni { get; }

    public DniAlreadyInUseException(Dni? dni)
        : base(dni is null ? "A DNI is already in use." : $"The DNI '{dni}' is already in use.")
    {
        Dni = dni?.ToString() ?? string.Empty;
    }

    public DniAlreadyInUseException(string? message, Exception? innerException = null)
        : base(message, innerException)
    {
        Dni = string.Empty;
    }
}