using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

public record GetLocalSubscriptionIdQuery(string StripeSubscriptionId) : IRequest<Guid?>;