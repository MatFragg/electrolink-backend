
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;

public partial class Profile
{
    public int Id { get; protected set; }
    public PersonName Name { get; protected set; }
    public EmailAddress Email { get; protected set; }
    public StreetAddress Address { get; protected set; }

    public Role Role { get; protected set; }

    public HomeOwner? HomeOwner { get; protected set; }
    public Technician? Technician { get; protected set; }

    public string FullName => Name.FullName;
    public string EmailAddress => Email.Address;
    public string StreetAddress => Address.FullAddress;

    protected Profile()
    {
        Name = new PersonName();
        Email = new EmailAddress();
        Address = new StreetAddress();
    }

    public Profile(string firstName, string lastName, string email, string street, string number, string city,
        string postalCode, string country, Role role)
    {
        Name = new PersonName(firstName, lastName);
        Email = new EmailAddress(email);
        Address = new StreetAddress(street, number, city, postalCode, country);
        Role = role;
    }

    public Profile(CreateProfileCommand command)
    {
        Name = new PersonName(command.FirstName, command.LastName);
        Email = new EmailAddress(command.Email);
        Address = new StreetAddress(command.Street, command.Number, command.City, command.PostalCode, command.Country);
        Role = command.Role;

        switch (Role)
        {
            case Role.HomeOwner:
                if (string.IsNullOrWhiteSpace(command.Dni))
                    throw new ArgumentException("Dni is required for HomeOwner role.");
                HomeOwner = new HomeOwner(command.Dni);
                break;

            case Role.Technician:
                if (string.IsNullOrWhiteSpace(command.LicenseNumber) || string.IsNullOrWhiteSpace(command.Specialization))
                    throw new ArgumentException("LicenseNumber and Specialization are required for Technician role.");
                Technician = new Technician(command.LicenseNumber, command.Specialization);
                break;

            default:
                throw new InvalidOperationException("Unsupported profile role.");
        }
    }

    public void AssignHomeOwnerInfo(string dni)
    {
        if (Role != Role.HomeOwner)
            throw new InvalidOperationException("Cannot assign HomeOwner info to a non-HomeOwner profile.");

        HomeOwner = new HomeOwner(dni);
    }

    public void AssignTechnicianInfo(string licenseNumber, string specialization)
    {
        if (Role != Role.Technician)
            throw new InvalidOperationException("Cannot assign Technician info to a non-Technician profile.");

        Technician = new Technician(licenseNumber, specialization);
    }
}
