using Hampcoders.Electrolink.API.Processing.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Processing.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Processing.Interfaces.REST.Transform;

public static class RelayHistoryResourceFromEntityAssembler
{
    public static RelayCommandHistoryResource ToResourceFromEntities(
        string deviceId,
        IEnumerable<RelayControlCommand> commands)
    {
        var list      = commands.ToList();
        var lastCmd   = list.OrderByDescending(c => c.IssuedAt).FirstOrDefault();
        var lastState = lastCmd?.Status == ERelayCommandStatus.Executed
            ? lastCmd.TargetRelayState.ToString()
            : "UNKNOWN";

        return new RelayCommandHistoryResource(
            deviceId, lastState, lastCmd?.IssuedAt,
            list.Select(c => new RelayCommandItemResource(
                c.CommandId.Value, c.TargetRelayState.ToString(),
                c.RequestedBy, c.AuthorizationSource,
                c.ServiceRequestId?.Value, c.Status.ToString(),
                c.IssuedAt, c.ExecutedAt, c.FailureReason)).ToList());
    }
}
