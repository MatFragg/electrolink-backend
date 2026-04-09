using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class CancelServiceExecutionCommandFromResourceAssembler
{
    public static CancelServiceExecutionCommand ToCommandFromResource(string executionId, string actorId, CancelServiceExecutionResource resource)
    {
        return new CancelServiceExecutionCommand(
            ServiceExecutionId.From(executionId),
            actorId,
            Enum.Parse<ECancelledBy>(resource.CancelledBy, true),
            resource.Reason,
            resource.Notes,
            resource.RequestReassignment
        );
    }
}

