using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class CompleteServiceExecutionCommandAssembler
{
    public static CompleteServiceExecutionCommand ToCommand(string executionId, string technicianId)
    {
        return new CompleteServiceExecutionCommand(
            ServiceExecutionId.From(executionId),
            TechnicianId.From(technicianId),
            System.DateTime.UtcNow
        );
    }
}

