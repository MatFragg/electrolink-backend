using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record PortfolioEntryId{
    public string Value { get; init; }

    private PortfolioEntryId(string value) => Value = value;

    public static PortfolioEntryId NewPortfolioEntryId() => new($"pentry-{Guid.NewGuid()}");

    public static PortfolioEntryId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("pentry-"))
            throw new InvalidIdException("PortfolioEntryId", value);
        return new PortfolioEntryId(value);
    }

    public override string ToString() => Value;
}