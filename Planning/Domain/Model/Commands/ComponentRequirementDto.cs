namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record ComponentRequirementDto(
    string ComponentTypeId,
    string ComponentTypeName,
    int Quantity,
    bool IsRequired
);

