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
    public async Task<IEnumerable<(string technicianId, string profileId, string fullName, double rating)>>
        GetTechniciansInAreaAsync(double latitude, double longitude)
        => await profilesContextFacade.GetTechniciansInAreaAsync(latitude, longitude);

    /// <summary>
    /// Checks if a homeowner profile is active based on the provided homeowner ID.
    /// </summary>
    /// <param name="homeownerId"></param>
    /// <returns>True or False depending on Homeowner's status</returns>
    public async Task<bool> IsHomeownerActiveAsync(string homeownerId)
        => await profilesContextFacade.IsHomeownerActiveAsync(homeownerId);
    
    public async Task<bool> HasPropertiesAsync(string homeownerId)
        => await profilesContextFacade.HomeownerHasPropertiesAsync(homeownerId);
    
    public async Task EnsureHomeownerIsActiveAsync(string homeownerId)
    {
        var isActive = await profilesContextFacade.IsHomeownerActiveAsync(homeownerId);
        if (!isActive)
            throw new InvalidOperationException(
                $"Homeowner {homeownerId} does not have an active profile.");
    }
    
    public async Task EnsureHasPropertiesAsync(string homeownerId)
    {
        var hasProperties = await profilesContextFacade.HomeownerHasPropertiesAsync(homeownerId);
        if (!hasProperties)
            throw new InvalidOperationException(
                $"Homeowner {homeownerId} has no registered properties.");
    }
}