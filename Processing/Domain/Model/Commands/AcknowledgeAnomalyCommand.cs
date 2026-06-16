namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;

public record AcknowledgeAnomalyCommand(
    string  AnomalyId,
    string  AcknowledgedByActorId
);
