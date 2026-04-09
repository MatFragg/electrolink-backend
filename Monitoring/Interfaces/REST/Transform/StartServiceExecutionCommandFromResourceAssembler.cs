using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class StartServiceExecutionCommandFromResourceAssembler
{
    public static StartServiceExecutionCommand ToCommandFromResource(string executionId, string technicianId, StartServiceExecutionResource resource)
    {
        return new StartServiceExecutionCommand(
            ServiceExecutionId.From(executionId),
            TechnicianId.From(technicianId),
            resource.StartedAt
        );
    }
}

