using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record AssignServiceToTechnicianCommand(
    RequestId RequestId,
    TechnicianId TechnicianId,
    RecipeId RecipeId,
    bool IsPriority
);
