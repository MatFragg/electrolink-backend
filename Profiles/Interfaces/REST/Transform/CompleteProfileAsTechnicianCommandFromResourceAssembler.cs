using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

public class CompleteProfileAsTechnicianCommandFromResourceAssembler
{
    public static CompleteProfileAsTechnicianCommand ToCommandFromResource(
        CompleteProfileAsTechnicianResource resource, 
        string userId) => 
        new(
            UserId:         userId,
            FirstName:      resource.FirstName,
            LastName:       resource.LastName,
            PhoneNumber:    resource.PhoneNumber,
            Dni:            resource.Dni,
            DateOfBirth:    resource.DateOfBirth,
            Street:         resource.Street,
            Number: resource.Number,
            District:       resource.District,
            City:           resource.City,
            Country:        resource.Country,
            PostalCode:     resource.PostalCode,
            Specialties:    resource.Specialties,
            ExperienceYears: resource.ExperienceYears,
            AboutMe:        resource.AboutMe
            );
}