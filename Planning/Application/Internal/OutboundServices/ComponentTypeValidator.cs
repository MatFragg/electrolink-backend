using Hampcoders.Electrolink.API.Assets.Interfaces.ACL;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;

public class ComponentTypeValidator(ExternalAssetsService externalAssetsService) : IComponentTypeValidator
{
    public void ValidateAll(IReadOnlyList<ComponentRequirementItem> requirements)
    {
        foreach (var req in requirements)
        {
            var exists = externalAssetsService.ComponentTypeExistsAndIsActiveAsync(req.ComponentTypeId)
                .GetAwaiter().GetResult();

            if (!exists)
                throw new InvalidComponentTypeException(req.ComponentTypeId);
        }
    }
}