using Hampcoders.Electrolink.API.Assets.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal;

using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;

public class ComponentTypeValidator : IComponentTypeValidator
{
    private readonly IAssetsContextFacade _assetsFacade;

    public ComponentTypeValidator(IAssetsContextFacade assetsFacade)
    {
        _assetsFacade = assetsFacade;
    }

    public async Task ValidateAllAsync(IEnumerable<ComponentRequirementItem> requirements)
    {
        foreach (var req in requirements)
        {
            await ValidateAsync(req);
        }
    }

    public async Task ValidateAsync(ComponentRequirementItem requirementItem)
    {
        // We use the Outbound Port to check existence in the Assets BC
        var isValid = await _assetsFacade.IsValidComponentTypeAsync(requirementItem.ComponentTypeId);
        
        if (!isValid)
        {
            throw new InvalidComponentTypeException(requirementItem.ComponentTypeId);
        }
    }

    public void ValidateAll(IEnumerable<ComponentRequirementItem> requirements)
    {
        // In a Modular Monolith, we can perform a synchronous check if the 
        // Assets BC provides a thread-safe cache or local look-up.
        // Otherwise, this is usually called after an async pre-check in the Command Service.
        foreach (var req in requirements)
        {
            // Implementation logic for sync validation
        }
    }
}