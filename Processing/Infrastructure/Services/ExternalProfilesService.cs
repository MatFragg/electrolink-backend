using Hampcoders.Electrolink.API.Processing.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Processing.Infrastructure.Services;

public class ExternalProfilesService(AppDbContext context) : IProfilesContextFacade
{
    public async Task<bool> IsTechnicianIoTCertifiedAsync(string technicianId)
    {
        var profile = await context.Profiles
            .Where(p => p.Technician != null && p.Technician.TechnicianId.Value == technicianId)
            .FirstOrDefaultAsync();

        return profile is not null && profile.Technician is not null;
    }
}
