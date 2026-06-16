using Hampcoders.Electrolink.API.Processing.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Processing.Infrastructure.Services;

/// <summary>
/// Implementation of IServiceOperationContextFacade that queries the
/// Monitoring BC's ServiceExecutions table via the shared AppDbContext.
/// TODO: When Monitoring BC exposes this via its own ContextFacade,
/// replace this with a direct call to that facade.
/// </summary>
public class ServiceOperationContextFacade(AppDbContext context)
    : IServiceOperationContextFacade
{
    private static readonly HashSet<string> ActiveStatuses = new()
    {
        "Notified", "EnRoute", "Arrived", "InProgress"
    };

    public async Task<bool> HasActiveServiceForPropertyAsync(string propertyId, string technicianId)
        => await context.ServiceExecutions.AnyAsync(se =>
            se.PropertyId.Value == propertyId &&
            se.TechnicianId.Value == technicianId &&
            ActiveStatuses.Contains(se.Status.ToString()));
}
