using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record CreateBillingPortalSessionCommand(
    UserId UserId,
    string ReturnUrl
);