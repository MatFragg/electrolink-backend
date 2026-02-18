namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ScheduledSlot
{
    public DateTime StartDateTime { get; init; }
    public DateTime EndDateTime { get; init; }
    
    public ScheduledSlot() : this(DateTime.MinValue, DateTime.MinValue) { }
    
    public ScheduledSlot(DateTime startDateTime, DateTime endDateTime)
    {
        if (endDateTime <= startDateTime)
            throw new ArgumentException("End must be after start");
        if (startDateTime < DateTime.UtcNow)
            throw new ArgumentException("Slot cannot be in the past");
            
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
    }
    
    public int DurationInMinutes() =>
        (int)(EndDateTime - StartDateTime).TotalMinutes;
    
    public bool Overlaps(ScheduledSlot other) =>
        StartDateTime < other.EndDateTime && EndDateTime > other.StartDateTime;
}

