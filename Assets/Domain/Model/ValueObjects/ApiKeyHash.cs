namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record ApiKeyHash
{
    public string Value { get; }

    private ApiKeyHash(string value) => Value = value;

    public static ApiKeyHash CreateFromPlainText(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key cannot be empty.");
        var hash = BCrypt.Net.BCrypt.HashPassword(apiKey);
        return new ApiKeyHash(hash);
    }

    public static ApiKeyHash From(string hashValue)
    {
        if (string.IsNullOrWhiteSpace(hashValue))
            throw new ArgumentException("ApiKeyHash cannot be empty.");
        return new ApiKeyHash(hashValue);
    }

    public bool Verify(string plainTextKey) => BCrypt.Net.BCrypt.Verify(plainTextKey, Value);
}
