namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class InvalidComponentTypeException(string componentTypeId)
    : Exception($"Invalid component type: {componentTypeId}");

