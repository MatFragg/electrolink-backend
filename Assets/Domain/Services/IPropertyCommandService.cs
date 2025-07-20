using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface IPropertyCommandService
{
    Task<Property?> Handle(CreatePropertyCommand command);
    /*Task<Property?> Handle(AddPhotoToPropertyCommand command);*/
    Task<Property?> Handle(UpdatePropertyAddressCommand command);
    Task<bool> Handle(DeletePropertyCommand command);
    Task<Property?> Handle(UpdatePropertyCommand command);
    
    //Task<Property?> Handle(DeactivatePropertyCommand command);
}