using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

public record UpdateServiceStatusCommand(Guid RequestId, ServiceStatus NewStatus);