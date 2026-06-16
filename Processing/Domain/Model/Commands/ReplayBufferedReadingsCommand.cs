namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;

public record ReplayBufferedReadingsCommand(
    string  DeviceId,
    IReadOnlyList<IngestDeviceReadingCommand> BufferedReadings
);
