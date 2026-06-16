using System.ComponentModel.DataAnnotations;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record OpenCustomerPortalResource([property: Required, Url] string ReturnUrl);

