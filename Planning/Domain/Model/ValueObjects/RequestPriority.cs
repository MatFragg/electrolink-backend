namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record RequestPriority
{
    public bool IsPriority { get; }
    public string Level { get; }

    private RequestPriority(bool isPriority, string level)
    {
        IsPriority = isPriority;
        Level = level;
    }

    public static readonly RequestPriority Standard = new(false, "Standard");
    public static readonly RequestPriority High = new(true, "High");

    public static RequestPriority FromPremiumStatus(bool isPremiumUser)
    {
        return isPremiumUser ? High : Standard;
    }

    public override string ToString() => Level;
}