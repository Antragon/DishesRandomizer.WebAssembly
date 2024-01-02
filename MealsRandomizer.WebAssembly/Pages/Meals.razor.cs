namespace MealsRandomizer.WebAssembly.Pages;

using Controllers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

public partial class Meals {
    private const int _pageSize = 10;

    private readonly List<Meal> _mealsSelection = new();

    private RadzenPager _pager = null!;

    [Inject] private DialogService DialogService { get; set; } = null!;

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    protected override void OnInitialized() {
        SetMealsSelection(0, _pageSize);
    }

    private Task AddMealAsync() {
        var newMeal = new Meal(Guid.NewGuid(), string.Empty, null);
        return OpenMealEditorAsync(newMeal, true);
    }

    private async Task OpenMealEditorAsync(Meal meal, bool isNew = false) {
        var parameters = new Dictionary<string, object> {
            [nameof(MealEditor.Meal)] = meal.Clone(),
            [nameof(MealEditor.IsNew)] = isNew
        };
        var dialogOptions = new DialogOptions { Style = "min-height:auto;min-width:auto;width:auto" };
        await DialogService.OpenAsync<MealEditor>(nameof(Meal), parameters, dialogOptions);
        await _pager.GoToPage(_pager.CurrentPage, true);
    }

    private async Task DeleteMealAsync(Meal meal) {
        var confirmOptions = new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "Cancel" };
        var result = await DialogService.Confirm("Are you sure?", "Delete Meal", confirmOptions);
        if (result.HasValue && result.Value) {
            CookbookController.DeleteMeal(meal.Id);
            await _pager.GoToPage(_pager.CurrentPage, true);
        }
    }

    private void PageChanged(PagerEventArgs args) {
        var skip = args.Skip;
        var take = args.Top;
        SetMealsSelection(skip, take);
    }

    private void SetMealsSelection(int skip, int take) {
        var meals = CookbookController.Cookbook.Meals.Values.OrderBy(meal => meal);
        _mealsSelection.Clear();
        _mealsSelection.AddRange(meals.Skip(skip).Take(take));
    }
}