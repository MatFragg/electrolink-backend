namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

public record CertificationData
{
    public string Name { get; init; }
    public string IssuerOrganization { get; init; }
    public DateTime DateObtained { get; init; }
    public DateTime? ExpirationDate { get; init; }
    public string? CredentialId { get; init; }
    public string? CredentialUrl { get; init; }

    private CertificationData() { }

    public static CertificationData Create(string name, string issuerOrganization, DateTime dateObtained,
        DateTime? expirationDate = null, string? credentialId = null, string? credentialUrl = null)
    {
        return new CertificationData
        {
            Name = name,
            IssuerOrganization = issuerOrganization,
            DateObtained = dateObtained,
            ExpirationDate = expirationDate,
            CredentialId = credentialId,
            CredentialUrl = credentialUrl
        };
    }

    public CertificationData Update(string? name = null, string? issuerOrganization = null,
        DateTime? dateObtained = null,
        DateTime? expirationDate = null, string? credentialId = null, string? credentialUrl = null) => this with
        {
            Name = name ?? this.Name,
            IssuerOrganization = issuerOrganization ?? this.IssuerOrganization,
            DateObtained = dateObtained ?? this.DateObtained,
            ExpirationDate = expirationDate ?? this.ExpirationDate,
            CredentialId = credentialId ?? this.CredentialId,
            CredentialUrl = credentialUrl ?? this.CredentialUrl
        };

}