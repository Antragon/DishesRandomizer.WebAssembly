namespace MealsRandomizer.WebAssembly.Components;

using Controllers;
using Microsoft.AspNetCore.Components;
using Pages;
using Radzen;

public partial class IngredientsCard {
    [Inject] private DialogService DialogService { get; set; } = null!;

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    private Meal? Meal => CookbookController.GetPlannedMeal(Day);
    private List<Ingredient> Ingredients => Meal?.Ingredients ?? new List<Ingredient>();

    [Parameter, EditorRequired] public Day Day { get; set; }

    private async Task OpenMealEditor() {
        if (Meal == null) return;
        var parameters = new Dictionary<string, object> {
            [nameof(MealEditor.Meal)] = Meal.Clone()
        };
        var dialogOptions = new DialogOptions { Style = "min-height:auto;min-width:auto;width:auto" };
        await DialogService.OpenAsync<MealEditor>(nameof(MealsRandomizer.Meal), parameters, dialogOptions);
        StateHasChanged();
    }
}