using System;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Represents a unique identifier for a Service Evaluation.
/// Format: eval-{GUID}
/// </summary>
public record EvaluationId
{
    public string Value { get; init; }

    private EvaluationId(string value) => Value = value;

    /// <summary>
    /// Generates a new unique EvaluationId.
    /// </summary>
    public static EvaluationId NewId() => new($"eval-{Guid.NewGuid()}");

    /// <summary>
    /// Creates an EvaluationId from an existing value with validation.
    /// </summary>
    public static EvaluationId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("eval-"))
            throw new InvalidIdException("EvaluationId", value);
        return new EvaluationId(value);
    }

    public override string ToString() => Value;
}

