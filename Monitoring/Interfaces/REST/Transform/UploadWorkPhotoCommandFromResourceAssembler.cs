using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class UploadWorkPhotoCommandFromResourceAssembler
{
    public static UploadWorkPhotoCommand ToCommandFromResource(string executionId, string technicianId, UploadWorkPhotoResource resource)
    {
        return new UploadWorkPhotoCommand(
            ServiceExecutionId.From(executionId),
            TechnicianId.From(technicianId),
            Enum.Parse<EPhotoType>(resource.PhotoType, true),
            resource.PhotoUrl,
            resource.TakenAt,
            resource.Notes
        );
    }
}

