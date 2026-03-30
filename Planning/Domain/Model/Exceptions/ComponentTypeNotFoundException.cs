namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class ComponentTypeNotFoundException(string componentTypeId)
    : Exception($"Component type not found: {componentTypeId}");

