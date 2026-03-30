namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

/// <summary>
/// Domain Service to validate that component types requested in a recipe 
/// are valid and exist in the system (Assets BC).
/// </summary>
public interface IComponentTypeValidator
{
    /// <summary>
    /// Validates a single component requirement.
    /// </summary>
    /// <exception cref="InvalidComponentTypeException">Thrown if the type is invalid or inactive.</exception>
    Task ValidateAsync(ComponentRequirement requirement);

    /// <summary>
    /// Validates a collection of component requirements.
    /// </summary>
    Task ValidateAllAsync(IEnumerable<ComponentRequirement> requirements);

    /// <summary>
    /// Synchronous version for use inside Aggregate Roots (via double dispatch).
    /// Note: In a real-world scenario, this might be pre-validated in the Application Service
    /// or implemented via a cached look-up.
    /// </summary>
    void ValidateAll(IEnumerable<ComponentRequirement> requirements);
}