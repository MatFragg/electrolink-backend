using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record AssignmentId
{
    public string Value { get; init; }

    private AssignmentId(string value) => Value = value;

    public static AssignmentId NewAssignmentId() => new($"assign-{Guid.NewGuid()}");

    public static AssignmentId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("assign-"))
            throw new InvalidIdException("AssignmentId", value);
        return new AssignmentId(value);
    }

    public override string ToString() => Value;
}