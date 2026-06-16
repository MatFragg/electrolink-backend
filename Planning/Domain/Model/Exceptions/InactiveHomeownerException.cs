namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

/// <summary>
/// Excepción lanzada cuando se intenta operar con un homeowner inactivo.
/// </summary>
public class InactiveHomeownerException(string homeownerId)
    : DomainException($"Homeowner {homeownerId} does not have an active profile.");
