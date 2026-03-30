using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record ReserveComponentsForServiceCommand(
    TechnicianId TechnicianId,
    AssignmentId AssignmentId,
    IReadOnlyList<ComponentAdjustment> ComponentsToReserve);