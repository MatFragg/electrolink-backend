namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

public record AddReportPhotoCommand(Guid ReportId, string Url, string Type);
