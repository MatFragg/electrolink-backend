namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;

public record ResolveAnomalyCommand(
    string  AnomalyId,
    int     ConsecutiveNormalReadings
);
