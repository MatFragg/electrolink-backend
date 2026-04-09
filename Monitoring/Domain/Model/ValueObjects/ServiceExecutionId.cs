using System;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Represents a unique identifier for a Service Execution.
/// Format: exec-{GUID}
/// </summary>
public record ServiceExecutionId
{
    public string Value { get; init; }

    private ServiceExecutionId(string value) => Value = value;

    /// <summary>
    /// Generates a new unique ServiceExecutionId.
    /// </summary>
    public static ServiceExecutionId NewId() => new($"exec-{Guid.NewGuid()}");

    /// <summary>
    /// Creates a ServiceExecutionId from an existing value with validation.
    /// </summary>
    public static ServiceExecutionId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("exec-"))
            throw new InvalidIdException("ServiceExecutionId", value);
        return new ServiceExecutionId(value);
    }

    public override string ToString() => Value;
}

