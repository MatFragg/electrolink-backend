using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;

/// <summary>
/// Service responsible for interacting with the external Profiles service to retrieve technician and homeowner information.
/// </summary>
public class ExternalProfilesService(IProfilesContextFacade profilesContextFacade)
{
    /// <summary>
    /// Gets a list of technicians in a specific area based on latitude and longitude.
    /// </summary>
    /// <param name="latitude">The latitude of the area to search for technicians.</param>
    /// <param name="longitude">The longitude of the area to search for technicians.</param>
    /// <returns>A list of tuples containing technician ID, profile ID, full name, and rating.</returns>
    public async Task<IEnumerable<(string technicianId, string profileId, string fullName)>>
        GetTechniciansInAreaAsync(double latitude, double longitude)
        => await profilesContextFacade.GetTechniciansInAreaAsync(latitude, longitude);

    public async Task<(string technicianId, double serviceAreaLat, double serviceAreaLon, int experienceYears, IEnumerable<string> specialties)>
        GetTechnicianDetailsAsync(string technicianId)
        => await profilesContextFacade.GetTechnicianDetailsAsync(technicianId);

    public async Task<string?> GetTechnicianIdByUserIdAsync(string userId)
        => await profilesContextFacade.GetTechnicianIdByUserIdAsync(userId);

    /// <summary>
    /// Checks if a homeowner profile is active based on the provided homeowner ID.
    /// </summary>
    /// <param name="homeownerId"></param>
    /// <returns>True or False depending on Homeowner's status</returns>
    public async Task<bool> IsHomeownerActiveAsync(string homeownerId)
        => await profilesContextFacade.IsHomeownerActiveAsync(homeownerId);
    
    public async Task EnsureHomeownerIsActiveAsync(string homeownerId)
    {
        var isActive = await profilesContextFacade.IsHomeownerActiveAsync(homeownerId);
        if (!isActive)
            throw new InactiveHomeownerException(homeownerId);
    }
    
}