using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

namespace Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class CreateReportPhotoCommandFromResourceAssembler
{
    public static AddReportPhotoCommand ToCommandFromResource(Guid reportId, CreateReportPhotoResource resource)
        => new(reportId, resource.Url, resource.Type);
}