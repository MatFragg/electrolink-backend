using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record RequestPreferencesDto(
    List<DateTime> PreferredDates,
    TimePreference TimePreference,
    string? ProblemDescription
);

