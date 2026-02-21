namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

public record CreateServiceOperationCommand(Guid RequestId, string TechnicianId);
