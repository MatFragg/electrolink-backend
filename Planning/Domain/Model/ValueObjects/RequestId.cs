namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record RequestId
{
    public string Value { get; }

    private RequestId(string value) => Value = value;

    public static RequestId NewId() => new($"req-{Guid.NewGuid()}");

    public static RequestId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("req-")) 
            throw new ArgumentException("RequestId cannot be empty");
             
        return new RequestId(value);
    }
    
    public override string ToString() => Value;
}