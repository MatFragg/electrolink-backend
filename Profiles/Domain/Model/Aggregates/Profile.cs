
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using InvalidOperationException = System.InvalidOperationException;

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
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents.AsReadOnly();

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

    public Profile(int userId, CreateProfileCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName)) throw new ArgumentException("First name and Last name cannot be empty.");
        if (string.IsNullOrWhiteSpace(command.Email)) throw new ArgumentException("Email address cannot be empty.");
        if (string.IsNullOrWhiteSpace(command.Street)) throw new ArgumentException("Street address cannot be empty.");
        
        Id = userId;
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

    public void SetIdAfterPersistence(int newId)
    {
        if (Id != 0 && Id != newId)
            throw new InvalidOperationException($"Profile ID has already been set to {Id} and cannot be changed to a different value ({newId}).");
        
        if (Id == 0)
            Id = newId;
        

        _domainEvents.Add(new ProfileCreatedEvent(Id, Email.Address, Name.FullName, Role, DateTime.UtcNow));

        if (HomeOwner != null && HomeOwner.ProfileId == 0)
        {
            HomeOwner.SetProfileId(Id);
            _domainEvents.Add(new HomeOwnerInfoAssignedEvent(Id, HomeOwner.Dni, DateTime.UtcNow));
        }
        if (Technician != null && Technician.ProfileId == 0)
        {
            Technician.SetProfileId(Id);
            _domainEvents.Add(new TechnicianInfoAssignedEvent(Id, Technician.Id, Technician.LicenseNumber, Technician.Specialization, DateTime.UtcNow));
        }
    }
    public void AssignHomeOwnerInfo(string dni)
    {
        if (Role != Role.HomeOwner)
            throw new InvalidOperationException("Cannot assign HomeOwner info to a non-HomeOwner profile.");
        if (HomeOwner == null)
            HomeOwner = new HomeOwner(Id, dni);
        else
            HomeOwner.UpdateDni(dni); 
        
        _domainEvents.Add(new HomeOwnerInfoAssignedEvent(Id, dni, DateTime.UtcNow));
    }

    public void AssignTechnicianInfo(string licenseNumber, string specialization)
    {
        if (Role != Role.Technician)
            throw new InvalidOperationException("Cannot assign Technician info to a non-Technician profile.");

        if (Technician == null)
            Technician = new Technician(Id,licenseNumber, specialization); 
        else 
            Technician.UpdateSpecialization(specialization);
        
        _domainEvents.Add(new TechnicianInfoAssignedEvent(Id, Technician.Id, licenseNumber, specialization, DateTime.UtcNow));
    }
    
    public void UpdateProfileInfo(string firstName, string lastName, string email, string street, string number, string city,
        string postalCode, string country)
    {
        var oldFullName = Name.FullName;
        var oldEmailAddress = Email.Address;
        var oldStreetAddress = Address.FullAddress;

        Name = new PersonName(firstName, lastName);
        Email = new EmailAddress(email);
        Address = new StreetAddress(street, number, city, postalCode, country);

        _domainEvents.Add(new ProfileUpdatedEvent(
            Id,
            oldFullName, Name.FullName,
            oldEmailAddress, Email.Address,
            oldStreetAddress, Address.FullAddress,
            DateTime.UtcNow
        ));
    }


    public void UpdateTechnicianCoverageArea(string newCoverageAreaDetails)
    {
        if (Role != Role.Technician || Technician == null)
            throw new InvalidOperationException("Only technicians can update coverage area.");
        
        _domainEvents.Add(new TechnicianCoverageAreaUpdatedEvent(
            Technician.Id,
            newCoverageAreaDetails,
            DateTime.UtcNow
        ));
    }

    public void UpdateTechnicianSpecialties(IReadOnlyList<string> newSpecialties)
    {
        if (Role != Role.Technician || Technician == null)
            throw new InvalidOperationException("Only technicians can update specialties.");
        
        Technician.UpdateSpecialization(string.Join(", ", newSpecialties)); 
        _domainEvents.Add(new TechnicianSpecialtyUpdatedEvent(
            Technician.Id,
            newSpecialties,
            DateTime.UtcNow
        ));
    }
    
    public PortfolioItem AddPortfolioItemToTechnician(string title, string description, string imageUrl)
    {
        if (Role != Role.Technician || Technician == null)
            throw new InvalidOperationException("Only technicians can manage portfolio items.");
        
        var newPortfolioItem = Technician.AddPortfolioItem(title, description, imageUrl);
        _domainEvents.Add(new PortfolioItemAddedEvent(
            Id,
            newPortfolioItem.WorkId,
            newPortfolioItem.Title,
            DateTime.UtcNow
        ));
        return newPortfolioItem;
    }
    
    public void UpdateTechnicianPortfolioItemDetails(Guid workId, string newTitle, string newDescription, string newImageUrl)
    {
        if (Role != Role.Technician || Technician == null)
            throw new InvalidOperationException("Only technicians can manage portfolio items.");

        Technician.UpdatePortfolioItemDetails(workId, newTitle, newDescription, newImageUrl);
        _domainEvents.Add(new PortfolioItemUpdatedEvent(
            Id,
            workId,
            newTitle,
            DateTime.UtcNow
        ));
    }

    public void RemoveTechnicianPortfolioItem(Guid workId)
    {
        if (Role != Role.Technician || Technician == null)
            throw new InvalidOperationException("Only technicians can manage portfolio items.");

        Technician.RemovePortfolioItem(workId);
        _domainEvents.Add(new PortfolioItemRemovedEvent(
            Id,
            workId,
            DateTime.UtcNow
        ));
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

