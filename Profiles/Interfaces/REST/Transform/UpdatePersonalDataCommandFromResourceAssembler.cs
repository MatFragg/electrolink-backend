using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

public static class UpdatePersonalDataCommandFromResourceAssembler
{
    public static UpdateProfilePersonalDataCommand ToCommandFromResource(
        UpdatePersonalDataResource resource,
        string profileId,
        string userId) =>
        new(
            ProfileId:   profileId,
            UserId:      userId,
            FirstName:   resource.FirstName,
            LastName:    resource.LastName,
            PhoneNumber: resource.PhoneNumber,
            Street:      resource.Street,
            District:    resource.District,
            City:        resource.City,
            Country:     resource.Country,
            PostalCode:  resource.PostalCode);
}

