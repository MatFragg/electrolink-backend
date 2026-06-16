namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

/// <summary>
/// Resultado del scoring de un candidato técnico en el algoritmo de matching.
/// Movido de Shared.Infrastructure a Shared.Domain para evitar dependencia Domain → Infrastructure.
/// </summary>
public record ScoredCandidate(string TechnicianId, double Score, string? Reasoning);
