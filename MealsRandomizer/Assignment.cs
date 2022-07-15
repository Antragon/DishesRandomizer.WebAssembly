namespace MealsRandomizer;

public record Assignment {
    public Ingredient? Ingredient { get; set; }
    public decimal? Amount { get; set; }
}