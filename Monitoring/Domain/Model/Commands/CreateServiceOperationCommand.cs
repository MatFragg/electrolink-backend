namespace Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

public record CreateServiceOperationCommand(Guid RequestId, int TechnicianId);
