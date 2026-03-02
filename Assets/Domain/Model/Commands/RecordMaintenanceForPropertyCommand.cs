using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record RecordMaintenanceForPropertyCommand(PropertyId PropertyId, ServiceId ServiceId, TechnicianId TechnicianId, string WorkSummary, DateTime CompletedAt);