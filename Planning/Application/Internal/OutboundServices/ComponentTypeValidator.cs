using Hampcoders.Electrolink.API.Assets.Interfaces.ACL;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;

/// <summary>
/// Valida que los tipos de componente existan y estén activos en el BC de Assets.
/// 
/// ⚠️ NOTA: Este validador hace sync-over-async porque es invocado desde métodos
/// síncronos del dominio (ServiceCatalog.AddRecipe, ServiceRecipe.Update).
/// La solución definitiva es mover esta validación al Application Layer (Command Service)
/// antes de invocar los métodos del dominio.
/// </summary>
public class ComponentTypeValidator(ExternalAssetsService externalAssetsService) : IComponentTypeValidator
{
    public void ValidateAll(IReadOnlyList<ComponentRequirementItem> requirements)
    {
        foreach (var req in requirements)
        {
            // TODO: Mover esta validación al Application Layer para evitar sync-over-async
            var exists = externalAssetsService.ComponentTypeExistsAndIsActiveAsync(req.ComponentTypeId)
                .GetAwaiter().GetResult();

            if (!exists)
                throw new InvalidComponentTypeException(req.ComponentTypeId);
        }
    }
}
