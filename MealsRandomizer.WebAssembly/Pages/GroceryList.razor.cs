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
            .GroupBy(a => a.Key)
            .Select(g => (g.Key, g.Sum(a => a.Value)))
            .Select(a => (CookbookController.Cookbook.Ingredients[a.Key].Name, a.Item2))
            .OrderBy(a => a.Name);
        _assignments.AddRange(assignments);
    }
}