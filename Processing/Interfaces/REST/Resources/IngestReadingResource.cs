namespace Hampcoders.Electrolink.API.Processing.Interfaces.REST.Resources;

public record IngestReadingResource(
    string   DeviceId,
    string   ReadingId,
    DateTime Timestamp,
    float    Voltage,
    float    Current,
    float    PowerFactor,
    float    Frequency,
    string   Source
);
