namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;

public record EvaluateReadingForAnomaliesCommand(
    string  StreamId,
    string  ReadingId
);
