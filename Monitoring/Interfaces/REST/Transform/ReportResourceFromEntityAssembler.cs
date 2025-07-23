using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

namespace Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public class ReportResourceFromEntityAssembler
{
    public static ReportPhotoResource ToResourceFromEntity(ReportPhoto photo)
    {
        return new ReportPhotoResource(
            photo.Url,
            photo.Type
        );
    }

    public static List<ReportPhotoResource> ToPhotoResourcesFromEntity(IEnumerable<ReportPhoto> photos)
    {
        return photos.Select(ToResourceFromEntity).ToList();
    }

    public static ReportResource ToResourceFromEntity(Report entity, IEnumerable<ReportPhoto> photos)
    {
        return new ReportResource(
            entity.RequestId,
            entity.Description,
            entity.Date,
            ToPhotoResourcesFromEntity(photos)
        );
    }
}
