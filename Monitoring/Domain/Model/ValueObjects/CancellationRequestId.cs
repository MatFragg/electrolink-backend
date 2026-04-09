using System;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Represents a unique identifier for a Service Cancellation Request.
/// Format: cxreq-{GUID}
/// </summary>
public record CancellationRequestId
{
    public string Value { get; init; }

    private CancellationRequestId(string value) => Value = value;

    /// <summary>
    /// Generates a new unique CancellationRequestId.
    /// </summary>
    public static CancellationRequestId NewId() => new($"cxreq-{Guid.NewGuid()}");

    /// <summary>
    /// Creates a CancellationRequestId from an existing value with validation.
    /// </summary>
    public static CancellationRequestId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("cxreq-"))
            throw new InvalidIdException("CancellationRequestId", value);
        return new CancellationRequestId(value);
    }

    public override string ToString() => Value;
}

