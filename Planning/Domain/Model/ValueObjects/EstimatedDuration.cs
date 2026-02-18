namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record EstimatedDuration
{
    public int TotalMinutes { get; init; }
    
    internal EstimatedDuration()
    {
        TotalMinutes = 0;
    }

    // Constructor público por minutos totales (valida > 0)
    public EstimatedDuration(int totalMinutes)
    {
        if (totalMinutes <= 0)
            throw new ArgumentException("Duration must be greater than 0", nameof(totalMinutes));
        TotalMinutes = totalMinutes;
    }

    // Constructor público por horas y minutos (valida horas/minutos y resultado > 0)
    public EstimatedDuration(int hours, int minutes)
    {
        if (hours < 0)
            throw new ArgumentException("Hours cannot be negative", nameof(hours));
        if (minutes < 0 || minutes >= 60)
            throw new ArgumentException("Minutes must be between 0 and 59", nameof(minutes));

        var total = checked(hours * 60 + minutes);
        if (total <= 0)
            throw new ArgumentException("Duration must be greater than 0");

        TotalMinutes = total;
    }

    public (int hours, int minutes) ToHoursAndMinutes() =>
        (TotalMinutes / 60, TotalMinutes % 60);
}

