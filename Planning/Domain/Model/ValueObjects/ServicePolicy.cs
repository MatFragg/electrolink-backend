namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ServicePolicy(
    string CancellationPolicy,
    string TermsAndConditions
);