using Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Processing.Domain.Model.Entities;

public class Reading
{
    public ReadingId   ReadingId   { get; private set; }
    public StreamId    StreamId    { get; private set; }
    public DateTime    Timestamp   { get; private set; }
    public float       Voltage     { get; private set; }
    public float       Current     { get; private set; }
    public float       PowerFactor { get; private set; }
    public float       Frequency   { get; private set; }
    public EReadingSource Source   { get; private set; }

    private Reading() { }

    public static Reading Create(
        ReadingId readingId,
        StreamId streamId,
        DateTime timestamp,
        float voltage,
        float current,
        float powerFactor,
        float frequency,
        EReadingSource source)
    {
        if (voltage < 0)
            throw new ArgumentException("Voltage cannot be negative.");
        if (current < 0 || current > 1000)
            throw new ArgumentException("Current must be between 0 and 1000 A.");
        if (powerFactor < 0 || powerFactor > 1)
            throw new ArgumentException("Power factor must be between 0 and 1.");
        if (frequency < 0)
            throw new ArgumentException("Frequency cannot be negative.");

        return new Reading
        {
            ReadingId   = readingId,
            StreamId    = streamId,
            Timestamp   = timestamp,
            Voltage     = voltage,
            Current     = current,
            PowerFactor = powerFactor,
            Frequency   = frequency,
            Source      = source,
        };
    }

    public float ApparentPowerKva => (Voltage * Current) / 1000f;
    public float ActivePowerKw    => ApparentPowerKva * PowerFactor;
}
