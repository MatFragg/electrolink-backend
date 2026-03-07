using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

/// <summary>
/// Resource de request para PATCH /me/technician.
/// Todos los campos son opcionales (actualización parcial).
/// Distinto de TechnicianDataResource (que tiene campos requeridos para el complete).
/// </summary>
public record UpdateTechnicianDataRequest(
    List<ESpecialty>? Specialties,
    int? ExperienceYears,
    string? AboutMe);

