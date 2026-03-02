using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

/// <summary>
/// Represents a command to create an inventory for a technician.
/// This command is used to initiate the process of creating an inventory record for a specific technician identified by their TechnicianId.
/// The command encapsulates the necessary information required to perform this action, allowing for a clear and structured way to handle the creation of technician inventories within the system.
/// </summary>
/// <param name="TechnicianId"></param>
public record CreateTechnicianInventoryCommand(TechnicianId TechnicianId);
