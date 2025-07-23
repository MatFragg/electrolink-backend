using System.Linq;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Entities;

namespace Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;

public partial class ServiceOperation
{ public void Handle(UpdateServiceStatusCommand command)
    {
        if (command.RequestId != RequestId) return;
        ChangeStatus(command.NewStatus);
    }
}