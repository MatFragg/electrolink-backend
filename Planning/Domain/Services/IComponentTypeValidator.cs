using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IComponentTypeValidator
{
    void ValidateAll(IReadOnlyList<ComponentRequirementItem> requirements);
}