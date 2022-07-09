namespace MealsRandomizer;

public record Meal(Guid Id, string Name) : IComparable<Meal> {
    public string Name { get; set; } = Name;
    
    public override string ToString() {
        return Name;
    }

    public int CompareTo(Meal? other) {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }
}