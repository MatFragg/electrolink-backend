namespace Hampcoders.Electrolink.API.Shared.Interfaces.REST.Resources;

public record ComponentRequirementResource(
    string ComponentTypeId,
    int Quantity,
    bool IsRequired
);
