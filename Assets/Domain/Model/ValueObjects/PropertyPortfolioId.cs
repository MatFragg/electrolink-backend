using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record PropertyPortfolioId
{
    public string Value { get; init; }

    private PropertyPortfolioId(string value) => Value = value;

    public static PropertyPortfolioId NewPropertyPortfolioId() => new($"ppf-{Guid.NewGuid()}");

    public static PropertyPortfolioId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("ppf-"))
            throw new InvalidIdException("PropertyPortfolioId", value);
        return new PropertyPortfolioId(value);
    }

    public override string ToString() => Value;
}