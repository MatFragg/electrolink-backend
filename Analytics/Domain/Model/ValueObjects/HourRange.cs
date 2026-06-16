namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record HourRange
{
    public int StartHour { get; }
    public int EndHour { get; }

    private HourRange(int startHour, int endHour)
    {
        if (startHour < 0 || startHour > 23)
            throw new ArgumentException("StartHour must be between 0 and 23.");
        if (endHour < 0 || endHour > 23)
            throw new ArgumentException("EndHour must be between 0 and 23.");
        StartHour = startHour;
        EndHour = endHour;
    }

    public static HourRange Of(int startHour, int endHour) => new(startHour, endHour);
}
