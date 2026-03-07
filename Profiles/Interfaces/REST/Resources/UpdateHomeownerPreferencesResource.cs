using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

/// <summary>
/// Resource de request para PATCH /me/homeowner.
/// Todos los campos son opcionales (actualización parcial).
/// </summary>
public record UpdateHomeownerPreferencesResource(
    bool? SmsNotifications,
    bool? EmailNotifications,
    bool? PushNotifications,
    EContactTime? PreferredContactTime,
    EmergencyContactResource? EmergencyContact);

