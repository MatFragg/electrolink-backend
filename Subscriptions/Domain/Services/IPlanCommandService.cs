using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

/// <summary>
/// <para>Interface for plan command services.</para>
/// <para>Handles creating, updating, and deleting plans, returning primitive IDs or void.</para>
/// </summary>
public interface IPlanCommandService
{
    /// <summary>
    /// <para>Handles the command to create a new plan.</para>
    /// </summary>
    /// <param name="command">The <see cref="CreatePlanCommand"/>.</param>
    /// <returns>The GUID of the created plan.</returns>
    Task<Guid> Handle(CreatePlanCommand command);

    /// <summary>
    /// <para>Handles the command to update an existing plan.</para>
    /// </summary>
    /// <param name="command">The <see cref="UpdatePlanCommand"/>.</param>
    /// <returns>The GUID of the updated plan, or null if not found.</returns>
    Task<Guid?> Handle(UpdatePlanCommand command);

    /// <summary>
    /// <para>Handles the command to delete a plan.</para>
    /// </summary>
    /// <param name="command">The <see cref="DeletePlanCommand"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Handle(DeletePlanCommand command); // Void return
}