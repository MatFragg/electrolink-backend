using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Assembler to convert CreateBillingPortalSessionResource to CreateBillingPortalSessionCommand.
/// </summary>
public static class CreateBillingPortalSessionCommandFromResourceAssembler
{
    /// <summary>
    /// Converts the resource to the command.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="resource">The resource to convert.</param>
    /// <returns>The corresponding command.</returns>
    public static CreateBillingPortalSessionCommand ToCommand(int userId,CreateBillingPortalSessionResource resource)
    {
        return new CreateBillingPortalSessionCommand(
            new UserId(userId),
            resource.ReturnUrl);
    }
}