namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

/// <summary>
/// Email address value object
/// </summary>
public record EmailAddress(string Address)
{
    public EmailAddress() : this(string.Empty) { }
}