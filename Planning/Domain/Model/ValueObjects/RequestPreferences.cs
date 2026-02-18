namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record RequestPreferences
{
    public List<DateTime> PreferredDates { get; init; }
    public TimePreference TimePreference { get; init; }
    public string? ProblemDescription { get; init; }
    
    public RequestPreferences() : this(
        new List<DateTime>(),
        TimePreference.Anytime,
        null
    ) { }
    
    public RequestPreferences(List<DateTime> preferredDates, TimePreference timePreference, string? problemDescription)
    {
        var minDate = DateTime.UtcNow.AddDays(2);
        var invalidDate = preferredDates.FirstOrDefault(d => d < minDate);
        if (invalidDate != default)
            throw new ArgumentException("Preferred dates must be at least 2 days in the future");
            
        PreferredDates = preferredDates;
        TimePreference = timePreference;
        ProblemDescription = problemDescription;
    }
}

public enum TimePreference
{
    Morning,
    Afternoon,
    Anytime
}

