namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

public record EmergencyContact
{
    public string Name { get; init; }
    public string Relationship { get; init; }
    public string PhoneNumber { get; init; }
    
    private EmergencyContact()
    {
    }
    
    public static EmergencyContact Create(string name, string relationship, string phoneNumber) =>
        new()
        {
            Name = name.Trim(),
            Relationship = relationship.Trim(),
            PhoneNumber = phoneNumber.Trim()
        };

    public EmergencyContact Update(string? name = null, string? relationship = null, string? phoneNumber = null) =>
        this with
        {
            Name = name?.Trim() ?? Name,
            Relationship = relationship?.Trim() ?? Relationship,
            PhoneNumber = phoneNumber?.Trim() ?? PhoneNumber
        };
}