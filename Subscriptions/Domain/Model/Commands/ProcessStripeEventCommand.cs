using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record ProcessStripeEventCommand(
    string StripeEventJson,
    string StripeSignatureHeader
) : IRequest;