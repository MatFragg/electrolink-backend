using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record MarkServiceRequestAsAssignedCommand(
    RequestId RequestId,
    AssignmentId AssignmentId,
    TechnicianId TechnicianId,
    RecipeSnapshot RecipeSnapshot
);