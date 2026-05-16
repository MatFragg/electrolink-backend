namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record OpenCustomerPortalCommand(string UserId, string ReturnUrl);
