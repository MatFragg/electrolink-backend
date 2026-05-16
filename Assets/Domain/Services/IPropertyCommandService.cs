using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface IPropertyCommandService
{
    Task<Property?> Handle(CreatePropertyCommand command);
    Task<Property?> Handle(UpdatePropertyCommand command);
    Task<Property?> Handle(UpdatePropertyAddressCommand command);
    Task<Property?> Handle(UpdatePropertyGeolocationCommand command);
    Task<Property?> Handle(DeactivatePropertyCommand command);
    Task<Property?> Handle(ArchivePropertyCommand command);
    Task<Property?> Handle(ActivatePropertyCommand command);
    Task<bool> Handle(DeletePropertyCommand command);
    Task<Property?> Handle(RecordMaintenanceForPropertyCommand command);
    Task<Property?> Handle(AddPhotoToPropertyCommand command);
}