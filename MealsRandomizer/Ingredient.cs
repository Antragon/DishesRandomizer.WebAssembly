namespace MealsRandomizer;

public record Ingredient {
    public string Name { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
}