namespace MealsRandomizer;

public record Ingredient(Guid Id, string Name) : IComparable<Ingredient> {
    public string Name { get; set; } = Name;

    public override string ToString() {
        return Name;
    }

    public int CompareTo(Ingredient? other) {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return string.Compare(Name, other.Name, StringComparison.Ordinal);
    }
}