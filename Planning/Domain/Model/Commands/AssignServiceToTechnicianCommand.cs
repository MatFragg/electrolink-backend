namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record AssignServiceToTechnicianCommand(
    Guid RequestId,
    Guid TechnicianId,
    Guid RecipeId,
    bool IsPriority
);

