﻿using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Cross-Bounded Context reference to Planning BC's RequestId.
/// Represents a service request identifier from the Planning context.
/// Format: req-{GUID}
/// </summary>
public record RequestId
{
    public string Value { get; init; }

    private RequestId(string value) => Value = value;

    /// <summary>
    /// Creates a RequestId from an existing value with validation.
    /// </summary>
    public static RequestId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("req-"))
            throw new InvalidIdException("RequestId", value);
        return new RequestId(value);
    }

    public override string ToString() => Value;
}

