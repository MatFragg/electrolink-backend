namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record RequestStatus
{
    public string Value { get; }

    private RequestStatus(string value)
    {
        Value = value;
    }

    public static readonly RequestStatus Pending = new("Pending");
    public static readonly RequestStatus Confirmed = new("Confirmed");
    public static readonly RequestStatus InProgress = new("InProgress");
    public static readonly RequestStatus Completed = new("Completed");
    public static readonly RequestStatus Cancelled = new("Cancelled");

    private static readonly Dictionary<string, List<string>> ValidTransitions = new()
    {
        { "Pending", new List<string> { "Confirmed", "Cancelled" } },
        { "Confirmed", new List<string> { "InProgress", "Cancelled" } },
        { "InProgress", new List<string> { "Completed", "Cancelled" } },
        { "Completed", new List<string>() },
        { "Cancelled", new List<string>() }
    };

    public bool CanTransitionTo(RequestStatus newStatus)
    {
        if (!ValidTransitions.ContainsKey(Value))
            return false;

        return ValidTransitions[Value].Contains(newStatus.Value);
    }

    public static RequestStatus FromString(string status)
    {
        return status switch
        {
            "Pending" => Pending,
            "Confirmed" => Confirmed,
            "InProgress" => InProgress,
            "Completed" => Completed,
            "Cancelled" => Cancelled,
            _ => throw new ArgumentException($"Invalid request status: {status}")
        };
    }

    public override string ToString() => Value;
}