using System.Text.Json.Serialization;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record EstimatedDuration
{
    public int TotalMinutes { get; init; }
    public int Hours => TotalMinutes / 60;
    public int RemainingMinutes => TotalMinutes % 60;

    private EstimatedDuration() { }

    [JsonConstructor]
    public EstimatedDuration(int totalMinutes)
    {
        TotalMinutes = totalMinutes;
    }

    public static EstimatedDuration FromHoursAndMinutes(int hours, int minutes)
    {
        var totalMinutes = (hours * 60) + minutes;
        
        if (totalMinutes <= 0)
            throw new InvalidDurationException("La duración debe ser mayor que 0.");
            
        return new EstimatedDuration(totalMinutes);
    }

    public static EstimatedDuration FromMinutes(int minutes)
    {
        if (minutes <= 0)
            throw new InvalidDurationException("La duración debe ser mayor que 0.");
            
        return new EstimatedDuration(minutes);
    }
}