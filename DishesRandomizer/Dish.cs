namespace DishesRandomizer.Common;

public record Dish(Guid Id, string Name) : IComparable<Dish> {
    public string Name { get; set; } = Name;
    
    public override string ToString() {
        return Name;
    }

    public int CompareTo(Dish? other) {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }
}