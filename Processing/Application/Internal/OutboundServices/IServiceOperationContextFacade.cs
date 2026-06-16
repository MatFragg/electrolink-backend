namespace Hampcoders.Electrolink.API.Processing.Application.Internal.OutboundServices;

public interface IServiceOperationContextFacade
{
    Task<bool> HasActiveServiceForPropertyAsync(string propertyId, string technicianId);
}
