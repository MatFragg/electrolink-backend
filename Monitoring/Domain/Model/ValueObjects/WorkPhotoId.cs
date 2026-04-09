using System;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

/// <summary>
/// Represents a unique identifier for a Work Photo.
/// Format: workphoto-{GUID}
/// </summary>
public record WorkPhotoId
{
    public string Value { get; init; }

    private WorkPhotoId(string value) => Value = value;

    /// <summary>
    /// Generates a new unique WorkPhotoId.
    /// </summary>
    public static WorkPhotoId NewId() => new($"workphoto-{Guid.NewGuid()}");

    /// <summary>
    /// Creates a WorkPhotoId from an existing value with validation.
    /// </summary>
    public static WorkPhotoId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("workphoto-"))
            throw new InvalidIdException("WorkPhotoId", value);
        return new WorkPhotoId(value);
    }

    public override string ToString() => Value;
}

