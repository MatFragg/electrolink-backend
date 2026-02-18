namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record RecipeSnapshot
{
    public Guid RecipeId { get; init; }
    public string ServiceName { get; init; }
    public ServiceCategory ServiceCategory { get; init; }
    public IReadOnlyCollection<ComponentRequirement> ComponentRequirements { get; init; }
    public Pricing Pricing { get; init; }
    public EstimatedDuration EstimatedDuration { get; init; }
    public WarrantyPeriod WarrantyPeriod { get; init; }
    public DateTime SnapshotTakenAt { get; init; }
    
    public RecipeSnapshot() : this(
        Guid.Empty,
        string.Empty,
        ServiceCategory.Repair,
        new List<ComponentRequirement>(),
        new Pricing(),
        new EstimatedDuration(),
        new WarrantyPeriod(),
        DateTime.UtcNow
    ) { }
    
    public RecipeSnapshot(
        Guid recipeId,
        string serviceName,
        ServiceCategory serviceCategory,
        IReadOnlyCollection<ComponentRequirement> componentRequirements,
        Pricing pricing,
        EstimatedDuration estimatedDuration,
        WarrantyPeriod warrantyPeriod,
        DateTime snapshotTakenAt)
    {
        RecipeId = recipeId;
        ServiceName = serviceName ?? string.Empty;
        ServiceCategory = serviceCategory;
        ComponentRequirements = componentRequirements ?? new List<ComponentRequirement>();
        Pricing = pricing ?? new Pricing();
        EstimatedDuration = estimatedDuration ?? new EstimatedDuration();
        WarrantyPeriod = warrantyPeriod ?? new WarrantyPeriod();
        SnapshotTakenAt = snapshotTakenAt;
    }
}

