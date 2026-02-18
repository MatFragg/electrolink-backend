namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

/// <summary>
/// Request Status following wizard multi-step pattern
/// Flow: Draft → PropertySelected → ReadyToConfirm → PendingAssignment → Assigned
/// </summary>
public enum RequestStatus
{
    Draft,
    PropertySelected,
    ReadyToConfirm,
    PendingAssignment,
    Assigned,
    Cancelled
}