namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;

public record ForceResolveAnomalyCommand(
    string  AnomalyId,
    string  ResolvedByActorId
);
