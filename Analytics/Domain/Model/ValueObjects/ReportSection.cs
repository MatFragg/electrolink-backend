namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record ReportSection
{
    public string SectionType { get; }
    public string ContentJson { get; }

    private ReportSection(string sectionType, string contentJson)
    {
        if (string.IsNullOrWhiteSpace(sectionType))
            throw new ArgumentException("SectionType cannot be empty.");
        SectionType = sectionType;
        ContentJson = contentJson ?? "{}";
    }

    public static ReportSection Of(string sectionType, string contentJson) =>
        new(sectionType, contentJson);
}
