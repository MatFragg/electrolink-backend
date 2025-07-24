namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record ReportResource(Guid RequestId, string Description, DateTime Date,  List<ReportPhotoResource> Photos);