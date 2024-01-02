namespace MealsRandomizer.WebAssembly.Pages;

using Controllers;
using Microsoft.AspNetCore.Components;

public partial class GroceryList {
    private readonly List<(string Ingredient, decimal? Amount)> _assignments = new();

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    protected override void OnInitialized() {
        var assignments = Enum
            .GetValues<Day>()
            .Select(CookbookController.GetPlannedMeal)
            .OfType<Meal>()
            .SelectMany(m => m.Ingredients)
            .GroupBy(a => a.Name)
            .Select(g => (g.Key, g.Sum(a => a.Amount)))
            .OrderBy(a => a.Key);
        _assignments.AddRange(assignments);
    }
}