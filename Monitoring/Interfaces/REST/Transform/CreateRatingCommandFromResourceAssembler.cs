using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class CreateRatingCommandFromResourceAssembler
{
    public static AddRatingCommand ToCommandFromResource(Guid requestId, CreateRatingResource resource)
        => new(requestId, resource.Score, resource.Comment, resource.RaterId, resource.TechnicianId);
}