using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

/// <summary>
/// Assembler for converting <see cref="UpdateSubscriptionStatusResource"/> to <see cref="UpdateSubscriptionStatusCommand"/>.
/// </summary>
public static class UpdateSubscriptionStatusCommandFromResourceAssembler
{
    /// <summary>
    /// Converts a <see cref="UpdateSubscriptionStatusResource"/> to a <see cref="UpdateSubscriptionStatusCommand"/>.
    /// </summary>
    /// <param name="subscriptionId">The ID of the subscription to update.</param>
    /// <param name="resource">The resource containing the new status.</param>
    /// <returns>The created command.</returns>
    public static UpdateSubscriptionStatusCommand ToCommandFromResource(Guid subscriptionId, UpdateSubscriptionStatusResource resource)
    {
        if (!Enum.TryParse(resource.NewStatus, true, out ESubscriptionStatus newStatus))
        {
            throw new ArgumentException($"Invalid NewStatus: {resource.NewStatus}");
        }
        return new UpdateSubscriptionStatusCommand(subscriptionId, newStatus);
    }
}