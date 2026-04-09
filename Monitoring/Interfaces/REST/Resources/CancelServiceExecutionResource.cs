namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record CancelServiceExecutionResource(
    string CancelledBy, 
    string Reason,
    string? Notes,
    bool RequestReassignment
);
