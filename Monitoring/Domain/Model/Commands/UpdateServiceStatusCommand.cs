using Hamcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

public record UpdateServiceStatusCommand(Guid RequestId, ServiceStatus NewStatus);