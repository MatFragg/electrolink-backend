using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

public record CompleteProfileAsTechnicianResource(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Dni,
    string DateOfBirth,
    string Street,
    string Number,
    string District,
    string City,
    string Country,
    string PostalCode,
    List<ESpecialty> Specialties,
    int ExperienceYears,
    string AboutMe);