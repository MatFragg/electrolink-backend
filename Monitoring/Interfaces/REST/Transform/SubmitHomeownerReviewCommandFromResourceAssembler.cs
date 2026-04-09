using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class SubmitHomeownerReviewCommandFromResourceAssembler
{
    public static SubmitHomeownerReviewCommand ToCommandFromResource(string executionId, string homeownerId, SubmitReviewResource resource)
    {
        var categoryDictionary = resource.Categories.ToDictionary(
            kvp => Enum.Parse<EEvaluationCategory>(kvp.Key, true),
            kvp => kvp.Value
        );
        return new SubmitHomeownerReviewCommand(
            ServiceExecutionId.From(executionId),
            HomeownerId.From(homeownerId),
            resource.Rating,
            resource.Comment,
            categoryDictionary,
            System.DateTime.UtcNow
        );
    }
}

