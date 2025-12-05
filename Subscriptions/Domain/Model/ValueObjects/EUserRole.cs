namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Enum for the type of user role.
/// </summary>
public enum EUserRole
{
    /// <summary>
    /// User is a homeowner.
    /// </summary>
    Homeowner,
    /// <summary>
    /// User is a technician.
    /// </summary>
    Technician,
    /// <summary>
    /// Applies to all user roles.
    /// </summary>
    All
}