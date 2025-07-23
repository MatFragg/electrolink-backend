namespace Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

public record AddReportCommand(Guid RequestId, string Description);