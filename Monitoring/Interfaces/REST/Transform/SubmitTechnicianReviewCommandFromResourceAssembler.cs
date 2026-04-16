using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class SubmitTechnicianReviewCommandFromResourceAssembler
{
    public static SubmitTechnicianReviewCommand ToCommandFromResource(string executionId, string technicianId, SubmitReviewResource resource)
    {
        var categoryDictionary = resource.Categories.ToDictionary(
            kvp => Enum.Parse<ETechnicianEvaluationCategory>(kvp.Key, true),
            kvp => kvp.Value
        );

        return new SubmitTechnicianReviewCommand(
            ServiceExecutionId.From(executionId),
            TechnicianId.From(technicianId),
            resource.Rating,
            resource.Comment,
            categoryDictionary,
            DateTime.UtcNow
        );
    }
}

