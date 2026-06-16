using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

/// <summary>
/// Candidato enriquecido con datos de perfil, recipe y scoring para el matching.
/// Movido de Entities a ValueObjects ya que es un DTO de lectura sin comportamiento.
/// </summary>
public record EnrichedCandidate(
    string TechnicianId,
    double Rating,
    ServiceRecipe Recipe,
    double DistanceKm,
    int ExperienceYears,
    IReadOnlyList<string> Specialties,
    bool IsIoTCertified,
    bool HasRequiredComponents,
    int ResponseTimeMinutesAvg
)
{
    public int CompletedServicesCount => ExperienceYears * 20;
}
