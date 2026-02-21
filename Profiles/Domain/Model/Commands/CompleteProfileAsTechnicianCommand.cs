using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record CompleteProfileAsTechnicianCommand(
    string UserId, 
    string FirstName, 
    string LastName, 
    string Email, 
    string PhoneNumber, 
    string Dni, 
    string DateOfBirth, 
    string Street, 
    string District, 
    string City, 
    string Country, 
    string PostalCode, 
    IEnumerable<ESpecialty> Specialties, 
    int ExperienceYears, 
    string AboutMe);