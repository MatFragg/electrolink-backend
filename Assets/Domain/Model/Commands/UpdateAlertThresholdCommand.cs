using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdateAlertThresholdCommand(TechnicianId TechnicianId, ComponentId ComponentId, int NewThreshold);
