namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ServiceName
{
    public string Value { get; init; } = string.Empty;
    
    internal ServiceName() {
        Value = string.Empty;
    }
    
    public ServiceName(string value)
    {
        Value = Validate(value);
    }
    
    private static string Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Service name cannot be empty");
        if (value.Length < 5 || value.Length > 150)
            throw new ArgumentException("Service name must be between 5 and 150 characters");
        return value.Trim();
    }
}

