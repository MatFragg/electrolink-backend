namespace Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;

/// <summary>
/// DTO para perfil de técnico
/// </summary>
public record TechnicianProfileDto(
    Guid TechnicianId,
    string FullName,
    double Rating,
    double Latitude,
    double Longitude,
    bool IsAvailable
);

/// <summary>
/// Anti-Corruption Layer para comunicación con Profiles Bounded Context
/// </summary>
public class ExternalProfilesService(ILogger<ExternalProfilesService> logger)
{
    /// <summary>
    /// Obtiene técnicos disponibles en una zona geográfica
    /// </summary>
    public async Task<List<TechnicianProfileDto>> GetTechniciansInZoneAsync(double latitude, double longitude, double radiusKm)
    {
        logger.LogInformation($"[Planning BC] ACL: Getting technicians in zone (lat: {latitude}, lon: {longitude}, radius: {radiusKm}km)");
        
        // TODO: Implementar búsqueda geográfica en Profiles BC
        // var technicians = await _profilesContextFacade.GetTechniciansNearLocationAsync(latitude, longitude, radiusKm);
        
        await Task.Delay(10);
        
        // Placeholder - retornar lista vacía
        return new List<TechnicianProfileDto>();
    }

    /// <summary>
    /// Obtiene el perfil completo de un técnico
    /// </summary>
    public async Task<TechnicianProfileDto?> GetTechnicianProfileAsync(Guid technicianId)
    {
        logger.LogInformation($"[Planning BC] ACL: Getting profile for technician {technicianId}");
        
        // TODO: Implementar obtención de perfil desde Profiles BC
        // var profile = await _profilesContextFacade.GetTechnicianProfileAsync(technicianId);
        
        await Task.Delay(10);
        
        // Placeholder
        return new TechnicianProfileDto(
            TechnicianId: technicianId,
            FullName: "John Doe",
            Rating: 4.5,
            Latitude: -12.0464,
            Longitude: -77.0428,
            IsAvailable: true
        );
    }

    /// <summary>
    /// Verifica si un técnico está disponible
    /// </summary>
    public async Task<bool> IsTechnicianAvailableAsync(Guid technicianId)
    {
        logger.LogInformation($"[Planning BC] ACL: Checking if technician {technicianId} is available");
        
        var profile = await GetTechnicianProfileAsync(technicianId);
        return profile?.IsAvailable ?? false;
    }

    /// <summary>
    /// Obtiene el nombre completo de un homeowner
    /// </summary>
    public async Task<string> GetHomeownerNameAsync(Guid homeownerId)
    {
        logger.LogInformation($"[Planning BC] ACL: Getting name for homeowner {homeownerId}");
        
        // TODO: Implementar desde Profiles BC
        // var profile = await _profilesContextFacade.GetHomeownerProfileAsync(homeownerId);
        // return profile?.FullName ?? "Unknown";
        
        await Task.Delay(10);
        
        return "Homeowner Name"; // Placeholder
    }
}

