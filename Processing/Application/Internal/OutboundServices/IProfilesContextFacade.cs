namespace Hampcoders.Electrolink.API.Processing.Application.Internal.OutboundServices;

public interface IProfilesContextFacade
{
    Task<bool> IsTechnicianIoTCertifiedAsync(string technicianId);
}
