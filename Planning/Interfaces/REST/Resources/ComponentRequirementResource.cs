namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record ComponentRequirementResource(
    string ComponentTypeId,
    int Quantity,
    bool IsRequired
);

