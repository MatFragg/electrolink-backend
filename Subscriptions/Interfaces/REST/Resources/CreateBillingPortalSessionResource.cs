namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Resource for creating a billing portal session.
/// </summary>
public record CreateBillingPortalSessionResource([property: Required, Url] string ReturnUrl);