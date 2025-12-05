using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

using Stripe;

public record ProcessStripeEventCommand(
    string StripeEventJson, 
    string StripeSignatureHeader
) : IRequest<Unit>;