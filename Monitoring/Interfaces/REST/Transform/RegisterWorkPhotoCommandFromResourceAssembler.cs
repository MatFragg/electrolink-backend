using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class RegisterWorkPhotoCommandFromResourceAssembler
{
    public static RegisterWorkPhotoCommand ToCommandFromResource(
        string executionId,
        string technicianId,
        RegisterWorkPhotoResource resource)
    {
        return new RegisterWorkPhotoCommand(
            ServiceExecutionId.From(executionId),
            TechnicianId.From(technicianId),
            resource.ProviderId,
            resource.PublicUrl,
            resource.ThumbnailUrl,
            Enum.Parse<EPhotoType>(resource.PhotoType, true),
            resource.Format,
            resource.SizeBytes,
            resource.TakenAt,
            resource.Notes);
    }
}
