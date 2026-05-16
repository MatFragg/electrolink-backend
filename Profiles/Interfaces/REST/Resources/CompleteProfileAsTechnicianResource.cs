using System.ComponentModel.DataAnnotations;
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
    string AboutMe,
    [Range(-90d, 90d, ErrorMessage = "Latitud inválida")]
    double CenterLatitude,
    [Range(-180d, 180d, ErrorMessage = "Longitud inválida")]
    double CenterLongitude,
    [Range(typeof(double), "0.000001", "100", ErrorMessage = "El radio debe estar entre 0 y 100 km")]
    double RadiusKm);