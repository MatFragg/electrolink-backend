using System;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Represents a unique identifier for a Component Usage Record.
/// Format: compu-{GUID}
/// </summary>
public record ComponentUsageId
{
    public string Value { get; init; }

    private ComponentUsageId(string value) => Value = value;

    /// <summary>
    /// Generates a new unique ComponentUsageId.
    /// </summary>
    public static ComponentUsageId NewId() => new($"compu-{Guid.NewGuid()}");

    /// <summary>
    /// Creates a ComponentUsageId from an existing value with validation.
    /// </summary>
    public static ComponentUsageId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("compu-"))
            throw new InvalidIdException("ComponentUsageId", value);
        return new ComponentUsageId(value);
    }

    public override string ToString() => Value;
}

