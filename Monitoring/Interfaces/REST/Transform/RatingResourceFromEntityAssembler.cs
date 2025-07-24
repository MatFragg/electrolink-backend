using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public class RatingResourceFromEntityAssembler
{
    public static RatingResource ToResourceFromEntity(Rating entity)
    {
        return new RatingResource(
            entity.RequestId,
            entity.Score,
            entity.Comment,
            entity.RaterId,
            entity.TechnicianId
        );
    }
}